using BenchmarkBerkeleyDB.HistorianAPI;
using BenchmarkBerkeleyDB.HistorianAPI.Metadata;
using GSF;
using GSF.Diagnostics;
using GSF.Snap;
using GSF.Snap.Filters;
using GSF.TimeSeries;
using GSF.TimeSeries.Adapters;
using openHistorian.Net;
using openHistorian.Snap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BerkeleyDB;

namespace BenchmarkBerkeleyDB
{
    class DataReader : IDisposable
    {
        #region [ Members ]

        // Historian Members
        private SnapDBClient m_historianClient;
        private IEnumerable<ulong> m_points;

        // CSV Members
        StreamReader m_reader;
        private ulong[] m_indexToPointIDLookup;
        private int m_currentLine;
        private int m_totalLines;

        // Common Members
        private Settings m_settings;
        public delegate bool ReadNext(out DataPoint[] points);
        private ReadNext m_readNext;
        private DateTime m_startTime;
        private DateTime m_endTime;
        private double m_timeRange;
        private DataPoint m_dataPoint;
        private DataPoint[] m_dataPoints;
        private Dictionary<ulong, DataPoint> m_dataBlock;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        public DataReader(Settings settings, Action<string> showMessage, LogPublisher log)
        {
            ShowMessage = showMessage;
            Log = log;
            m_settings = settings;
            if (m_settings.ReadFromOpenHistorian)
            {
                m_startTime = new DateTime(m_settings.StartTime.Ticks, m_settings.UseUTCTime ? DateTimeKind.Utc : DateTimeKind.Local);
                m_endTime = new DateTime(m_settings.EndTime.Ticks, m_settings.UseUTCTime ? DateTimeKind.Utc : DateTimeKind.Local);

                // Convert all times to UTC since that's what the openHistorian uses.
                m_startTime = m_startTime.ToUniversalTime();
                m_endTime = m_endTime.ToUniversalTime();
                m_timeRange = (m_endTime - m_startTime).TotalSeconds;

                m_points = GetMetadata().Select(md => md.PointID);

                m_historianClient = new SnapDBClient(m_settings.HostAddress, m_settings.DataPort, m_settings.InstanceName, m_startTime, m_endTime, m_settings.FrameRate, m_points);

                m_dataBlock = new Dictionary<ulong, DataPoint>();
                m_dataPoint = new DataPoint();
                PointCount = m_points.Count();
                m_readNext = ReadNextHistorianBlock;

                if (!m_historianClient.ReadNext(m_dataPoint)) //Scan to first record in time range
                    throw new InvalidOperationException("No data for specified time range in openHistorian connection");

                CurrentTimestamp = new DateTime((long)m_dataPoint.Timestamp);

            }

            else if (m_settings.ReadFromCsv)
            {
                m_readNext = ReadNextCsvBlock;
                m_reader = new StreamReader(m_settings.CsvSourcePath);

                string headerLine = m_reader.ReadLine();
                string[] headers = headerLine.Split(',');

                m_currentLine = 0;

                IEnumerable<string> dataFile = File.ReadLines(m_settings.CsvSourcePath);
                m_totalLines = dataFile.Count();
                m_startTime = DateTime.Parse(dataFile.ElementAt(1).Split(',')[0]);
                m_endTime = DateTime.Parse(dataFile.ElementAt(m_totalLines - 1).Split(',')[0]);
                m_timeRange = (m_endTime - m_startTime).TotalSeconds;

                m_indexToPointIDLookup = new ulong[headers.Length];
                for (int i = 1; i < headers.Length; i++)
                    m_indexToPointIDLookup[i] = ParsePointID(headers[i].Trim());

                PointCount = headers.Length - 1;
                m_dataPoints = new DataPoint[PointCount];
                Parallel.For(0, PointCount, i => m_dataPoints[i] = new DataPoint());
            }

            Read = m_readNext;
            PercentComplete = 0;
        }

        #endregion

        #region [ Properties ]

        public Action<string> ShowMessage { get; set; }

        public LogPublisher Log { get; set; }

        public DateTime CurrentTimestamp { get; set; }

        public int PercentComplete { get; set; }

        public ReadNext Read { get; set; }

        public int PointCount { get; }

        #endregion

        #region [ Methods ]

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        if (m_settings.ReadFromOpenHistorian)
                            m_historianClient.Dispose();

                        else if (m_settings.ReadFromCsv)
                            m_reader.Dispose();
                    }
                }
                finally
                {
                    m_disposed = true; // Prevent duplicate dispose
                }
            }
        }

        public bool ReadNextHistorianBlock(out DataPoint[] points)
        {
            try
            {
                CurrentTimestamp = new DateTime((long)m_dataPoint.Timestamp);
                ulong currentTimestamp = m_dataPoint.Timestamp;

                int timeComparison;
                bool readSuccess = true;

                // Create a new data block for current timestamp and load first/prior point
                m_dataBlock.Clear();
                m_dataBlock[m_dataPoint.PointID] = m_dataPoint.Clone();

                // Load remaining data for current timestamp
                do
                {
                    // Scan to next record
                    if (!m_historianClient.ReadNext(m_dataPoint))
                    {
                        readSuccess = false;
                        break;
                    }

                    timeComparison = DataPoint.CompareTimestamps(m_dataPoint.Timestamp, currentTimestamp, m_settings.FrameRate);

                    if (timeComparison == 0)
                    {
                        m_dataBlock[m_dataPoint.PointID] = m_dataPoint.Clone();
                    }
                }
                while (timeComparison == 0);

                // Finished with data read
                if (!readSuccess)
                {
                    ShowMessage(">>> End of data in range encountered...");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowMessage($"!!! Failure during historian read: {ex.Message}");
                Log.Publish(MessageLevel.Error, "HistorianDataRead", "Failed while reading data from the historian", exception: ex);
                return false;
            }
            finally
            {
                PercentComplete = (int)((1.0D - (new Ticks(m_endTime.Ticks - (long)m_dataPoint.Timestamp).ToSeconds() / m_timeRange)) * 100.0D);
                points = m_dataBlock.Values.ToArray();
            }

        }

        public bool ReadNextCsvBlock(out DataPoint[] points)
        {
            string line;
            line = m_reader.ReadLine();
            if (line != null)
            {
                string[] values = line.Split(',');
                CurrentTimestamp = DateTime.Parse(values[0]);

                // Not as fast
                //float value2;
                //Parallel.For(1, values.Length, (i =>
                //{
                //    m_dataPoints[i - 1].PointID = m_indexToPointIDLookup[i];
                //    m_dataPoints[i - 1].ValueAsSingle = 0;

                //    if (float.TryParse(values[i - 1], out value2))
                //        m_dataPoints[i - 1].ValueAsSingle = value2;
                //}));

                float value;
                for (int i = 1; i < values.Length; i++)
                {
                    m_dataPoints[i - 1].Timestamp = (ulong)CurrentTimestamp.Ticks;
                    m_dataPoints[i - 1].PointID = m_indexToPointIDLookup[i];
                    m_dataPoints[i - 1].ValueAsSingle = 0;
                    if (float.TryParse(values[i], out value))
                        m_dataPoints[i - 1].ValueAsSingle = value;
                }

                points = m_dataPoints;
                PercentComplete = (++m_currentLine * 100) / m_totalLines;
                return true;
            }

            points = null;
            return false;
        }

        public Ticks ReadBackHistorianData(HistorianIArchive archive, Action<int> updateProgressBar)
        {
            IEnumerable<ulong> points;

            if (m_settings.ReadFromCsv)
                points = m_indexToPointIDLookup.Skip(1); // First value is always 0 because the timestamp is the first column
            else
                points = m_points;

            if (points == null)
            {
                ShowMessage("Point list not initialized");
                return new Ticks(0);
            }

            int count = 0;
            HistorianKey key = new HistorianKey();
            HistorianValue value = new HistorianValue();
            TreeStream<HistorianKey, HistorianValue> m_stream;

            SeekFilterBase<HistorianKey> timeFilter = TimestampSeekFilter.CreateFromRange<HistorianKey>(DataPoint.RoundTimestamp(m_startTime, m_settings.FrameRate), DataPoint.RoundTimestamp(m_endTime, m_settings.FrameRate));
            MatchFilterBase<HistorianKey, HistorianValue> pointFilter = PointIdMatchFilter.CreateFromList<HistorianKey, HistorianValue>(points);


            m_stream = archive.ClientDatabase.Read(GSF.Snap.Services.Reader.SortedTreeEngineReaderOptions.Default, timeFilter, pointFilter);


            int messageInterval = points.Count() * m_settings.MessageInterval;

            DateTime startTime = DateTime.UtcNow;
            while (m_stream.Read(key, value))
            {
                count++;

                if (count % messageInterval == 0)
                {
                    PercentComplete = (int)((1.0D - (new Ticks(m_endTime.Ticks - (long)key.Timestamp).ToSeconds() / m_timeRange)) * 100.0D);
                    ShowMessage($"{Environment.NewLine}{count} points read back so far, averaging {(count / (DateTime.UtcNow - startTime).TotalSeconds):N0} points per second.");
                    updateProgressBar(PercentComplete);
                }
            }

            return DateTime.UtcNow - startTime;
        }

        public Ticks ReadBackBerkeleyDBData(BTreeDatabase database, Action<int> updateProgressBar)
        {
            IEnumerable<ulong> points;
            if (m_settings.ReadFromCsv)
                points = m_indexToPointIDLookup.Skip(1); // First value is always 0 because the timestamp is the first column
            else
                points = m_points;

            if (points == null)
            {
                ShowMessage("Point list not initialized");
                return new Ticks(0);
            }

            int count = 0;
            ulong value;
            long timestamp;

            int messageInterval = m_settings.MessageInterval * points.Count();

            using (BTreeCursor cursor = database.Cursor())
            {
                DateTime startTime = DateTime.UtcNow;
                while (cursor.MoveNextMultipleKey())
                {
                    using (MultipleKeyDatabaseEntry pairs = cursor.CurrentMultipleKey)
                    {
                        foreach (KeyValuePair<DatabaseEntry, DatabaseEntry> p in pairs)
                        {
                            timestamp = BitConverter.ToInt64(p.Key.Data, 0);
                            value = BitConverter.ToUInt64(p.Value.Data, 0);
                            p.Key.Dispose();
                            p.Value.Dispose();
                            count++;

                            if (count % messageInterval == 0)
                            {
                                PercentComplete = (int)((1.0D - (new Ticks(m_endTime.Ticks - timestamp).ToSeconds() / m_timeRange)) * 100.0D);
                                ShowMessage($"{Environment.NewLine}{count} points read back so far, averaging {(count / (DateTime.UtcNow - startTime).TotalSeconds):N0} points per second.");
                                updateProgressBar(PercentComplete);
                            }
                        }
                    }
                }

                return (DateTime.UtcNow - startTime);
            }
        }

        #region [ Private Helper Functions ]

        private List<MetadataRecord> GetMetadata()
        {
            Ticks operationTime;
            Ticks operationStartTime;

            // Load historian meta-data
            ShowMessage(">>> Loading source connection metadata...");

            operationStartTime = DateTime.UtcNow.Ticks;
            List<MetadataRecord> metadata = MetadataRecord.Query(m_settings.HostAddress, m_settings.MetadataPort, m_settings.MetadataTimeout);
            operationTime = DateTime.UtcNow.Ticks - operationStartTime;

            ShowMessage("*** Metadata Load Complete ***");
            ShowMessage($"Total metadata load time {operationTime.ToElapsedTimeString(3)}...");

            // Parse meta-data expression
            ShowMessage(">>> Processing filter expression for metadata...");
            operationStartTime = DateTime.UtcNow.Ticks;
            MeasurementKey[] inputKeys = AdapterBase.ParseInputMeasurementKeys(MetadataRecord.Metadata, false, m_settings.PointList, "MeasurementDetail");
            List<ulong> pointIDList = inputKeys.Select(key => (ulong)key.ID).ToList();
            List<MetadataRecord> records = new List<MetadataRecord>();

            foreach (ulong pointID in pointIDList)
            {
                MetadataRecord record = metadata.FirstOrDefault(md => md.PointID == pointID);

                if ((object)record != null)
                    records.Add(record);
            }

            operationTime = DateTime.UtcNow.Ticks - operationStartTime;

            ShowMessage($">>> Historian read will be for {pointIDList.Count:N0} points based on provided meta-data expression.");

            ShowMessage("*** Filter Expression Processing Complete ***");
            ShowMessage($"Total filter expression processing time {operationTime.ToElapsedTimeString(3)}...");

            return records;
        }

        private ulong ParsePointID(string header)
        {

            char[] firstPart = header.TakeWhile(ch => ch != ']').ToArray();

            string number = "";

            for (int i = 2; i < firstPart.Length; i++)
            {
                number += firstPart[i];
            }

            ulong value;
            if (ulong.TryParse(number, out value))
                return value;

            return 0;
        }

        #endregion

        #endregion
    }
}
