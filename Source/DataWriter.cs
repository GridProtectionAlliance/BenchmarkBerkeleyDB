using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using BerkeleyDB;
using GSF.Diagnostics;
using BenchmarkBerkeleyDB.HistorianAPI;
using BenchmarkBerkeleyDB.HistorianAPI.Metadata;
using System.Diagnostics;
using System.IO;
using GSF;
using GSF.IO;
using GSF.Units;
using System.Threading.Tasks;
using System.Threading;
using GSF.Snap.Services;
using openHistorian.Snap;
using openHistorian.Net;

namespace BenchmarkBerkeleyDB
{
    /// <summary>
    /// Defines algorithm to be executed during historian read.
    /// </summary>
    public class DataWriter : IDisposable
    {
        #region [ Members ]

        // Nested Types

        // Meta-data fields
        private List<MetadataRecord> m_metadata;

        // Algorithm analytic fields
        private BTreeDatabaseConfig m_berkeleyDbCfg;
        private BTreeDatabase m_berkeleyDb;

        private Settings m_settings;

        // Algorithm processing statistic fields
        private long m_processedDataBlocks;

        private bool m_disposed;

        private DatabaseEntry m_berkeleyDbKey;
        private DatabaseEntry m_berkeleyDbValue;
        private KeyValuePair<DatabaseEntry, DatabaseEntry>[] m_berkeleyDbPointList;

        private HistorianServer m_historianServer;
        private HistorianIArchive m_historianArchive;
        private HistorianKey m_key;
        private HistorianValue m_value;

        #endregion

        #region [ Constructors ]

        public DataWriter(Settings settings, int pointCount)
        {
            m_settings = settings;
            if (m_settings.WriteToOpenHistorian) // Initialize OH instance
            {
                HistorianServerDatabaseConfig archiveInfo = new HistorianServerDatabaseConfig(settings.HistorianName, settings.HistorianArchive, true)
                {
                    TargetFileSize = (long)(1 * SI.Giga), // Just because
                    DirectoryMethod = ArchiveDirectoryMethod.TopDirectoryOnly,
                    StagingCount = 3,
                    DiskFlushInterval = 1000, // Smallest available time interval
                    CacheFlushInterval = 1000 // Largest available value
                };

                m_historianServer = new HistorianServer(archiveInfo, 38406);
                m_historianArchive = m_historianServer[settings.HistorianName];
                m_key = new HistorianKey();
                m_value = new HistorianValue();
            }
            else if (m_settings.WriteToBerkeleyDB) // Initialize BDB instance
            {
                m_berkeleyDbCfg = new BTreeDatabaseConfig()
                {
                    BTreeCompare = BerkeleyDBKeyComparison,
                    Creation = CreatePolicy.IF_NEEDED
                };
                m_berkeleyDb = BTreeDatabase.Open(Path.Combine(settings.HistorianArchive, settings.HistorianName), m_berkeleyDbCfg);

                m_berkeleyDbKey = new DatabaseEntry();
                m_berkeleyDbValue = new DatabaseEntry();

                m_berkeleyDbPointList = new KeyValuePair<DatabaseEntry, DatabaseEntry>[pointCount];
                Parallel.For(0, m_berkeleyDbPointList.Length, (i) => m_berkeleyDbPointList[i] = new KeyValuePair<DatabaseEntry, DatabaseEntry>(
                                                                                new DatabaseEntry(),
                                                                                new DatabaseEntry()));
            }
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets UI message display function.
        /// </summary>
        public Action<string> ShowMessage { get; set; }

        /// <summary>
        /// Gets or sets current message display interval.
        /// </summary>
        public int MessageInterval { get; set; }

        /// <summary>
        /// Gets or sets current logging publisher.
        /// </summary>
        public LogPublisher Log { get; set; }

        /// <summary>
        /// Gets or sets received historian meta-data.
        /// </summary>
        public List<MetadataRecord> Metadata
        {
            get
            {
                return m_metadata;
            }
            set
            {
                // Cache meta-data in case algorithm needs it
                m_metadata = value;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases all the resources used by the <see cref="DataWriter"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DataWriter"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        if (m_settings.WriteToOpenHistorian)
                            m_historianServer.Dispose();

                        else if (m_settings.WriteToBerkeleyDB)
                            m_berkeleyDb.Close();
                    }
                }
                finally
                {
                    m_disposed = true;  // Prevent duplicate dispose.
                }
            }
        }

        /// <summary>
        /// Default data processing entry point for <see cref="DataWriter"/>.
        /// </summary>
        /// <param name="timestamp">Timestamp of <paramref name="dataBlock"/>.</param>
        /// <param name="dataBlock">Points values read at current timestamp.</param>
        public void Write(DateTime timestamp, DataPoint[] dataBlock)
        {
            if (m_settings.WriteToOpenHistorian) // Write to openHistorian
            {
                foreach (DataPoint point in dataBlock)
                {
                    m_key.Timestamp = point.Timestamp;
                    m_key.PointID = point.PointID;

                    m_value.Value1 = point.Value;
                    m_value.Value3 = point.Flags;

                    m_historianArchive.Write(m_key, m_value);
                }
            }

            else if (m_settings.WriteToBerkeleyDB) // Write to Berkeley DB
            {
                //// Create a new pointList each time *slow
                //KeyValuePair<DatabaseEntry, DatabaseEntry>[] pointList = new KeyValuePair<DatabaseEntry, DatabaseEntry>[dataBlock.Length];

                //Parallel.For(0, pointList.Length, (i) => pointList[i] = new KeyValuePair<DatabaseEntry, DatabaseEntry>(
                //                                                            new DatabaseEntry(BitConverter.GetBytes(timestamp.Ticks).Concat(BitConverter.GetBytes(dataBlock[i].PointID)).ToArray()),
                //                                                            new DatabaseEntry(BitConverter.GetBytes(dataBlock[i].Value))));

                //m_db.Put(new MultipleKeyDatabaseEntry(pointList, false));


                //// Single writes using the same key and value each time *medium
                //foreach (DataPoint point in dataBlock)
                //{
                //    m_key.Data = BitConverter.GetBytes(timestamp.Ticks).Concat(BitConverter.GetBytes(point.PointID)).ToArray();
                //    m_value.Data = BitConverter.GetBytes(point.Value).ToArray();

                //    m_db.Put(m_key, m_value);
                //}

                // Bulk writes using the same pointList *fastest method found so far*
                Parallel.For(0, dataBlock.Length, (i) =>
                {
                    m_berkeleyDbPointList[i].Key.Data = BitConverter.GetBytes(timestamp.Ticks).Concat(BitConverter.GetBytes(dataBlock[i].PointID)).ToArray();
                    m_berkeleyDbPointList[i].Value.Data = BitConverter.GetBytes(dataBlock[i].Value);
                });

                m_berkeleyDb.Put(new MultipleKeyDatabaseEntry(m_berkeleyDbPointList.Take(dataBlock.Length), false));
            }
        }


        /// <summary>
        /// Read back the database created by the algorithm and time the operation
        /// </summary>
        public void ReadDb()
        {
            BTreeCursor cursor;
            int count = 0;
            DateTime startTime = DateTime.Now;
            float value;

            cursor = m_berkeleyDb.Cursor();

            while (cursor.MoveNextMultipleKey())
            {
                MultipleKeyDatabaseEntry pairs = cursor.CurrentMultipleKey;

                foreach (KeyValuePair<DatabaseEntry, DatabaseEntry> p in pairs)
                {
                    value = BitConverter.ToUInt64(p.Value.Data, 0);
                    count++;
                }
            }

            double seconds = (DateTime.Now - startTime).TotalSeconds;

            ShowMessage($"{count} points read in {seconds} seconds. Averaging {(int)Math.Round(count / seconds)} points per second.");
        }

        /// <summary>
        /// The function BerkeleyDB will use to compare keys in a BTree database
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        int BerkeleyDBKeyComparison(DatabaseEntry key1, DatabaseEntry key2)
        {
            long timestamp1 = BitConverter.ToInt64(key1.Data, 0);
            long timestamp2 = BitConverter.ToInt64(key2.Data, 0);

            if (timestamp1 != timestamp2)
                return timestamp1 > timestamp2 ? 1 : -1;

            ulong pointID1 = BitConverter.ToUInt64(key1.Data, sizeof(ulong));
            ulong pointID2 = BitConverter.ToUInt64(key2.Data, sizeof(ulong));

            if (pointID1 != pointID2)
                return pointID1 > pointID2 ? 1 : -1;

            return 0;
        }

        #endregion
    }
}
