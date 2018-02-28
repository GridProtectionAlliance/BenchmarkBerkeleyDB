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
using GSF.Snap.Filters;

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

        // BerkeleyDB Members
        DatabaseEnvironment m_env;
        private DatabaseEntry m_berkeleyDbKey;
        private DatabaseEntry m_berkeleyDbValue;
        private KeyValuePair<DatabaseEntry, DatabaseEntry>[] m_berkeleyDbPointList;

        // openHistorian Members
        private HistorianServer m_historianServer;
        private HistorianIArchive m_historianArchive;
        private HistorianKey m_historianKey;
        private HistorianValue m_historianValue;

        #endregion

        #region [ Constructors ]

        public DataWriter(Settings settings, int pointCount)
        {
            m_settings = settings;
            if (m_settings.WriteToOpenHistorian) // Initialize OH instance
            {
                string historianName = "DestinationHistorian";
                HistorianServerDatabaseConfig archiveInfo = new HistorianServerDatabaseConfig(historianName, settings.HistorianArchive, true)
                {
                    TargetFileSize = (long)(1 * SI.Giga), // Just because
                    DirectoryMethod = ArchiveDirectoryMethod.TopDirectoryOnly,
                    StagingCount = 3,
                    DiskFlushInterval = 1000, // Smallest available time interval
                    CacheFlushInterval = 1000 // Largest available value
                };

                m_historianServer = new HistorianServer(archiveInfo, m_settings.DestinationHistorianDataPort);
                m_historianArchive = m_historianServer[historianName];
                m_historianKey = new HistorianKey();
                m_historianValue = new HistorianValue();
            }
            else if (m_settings.WriteToBerkeleyDB) // Initialize BDB instance
            {
                m_berkeleyDbCfg = new BTreeDatabaseConfig()
                {
                    BTreeCompare = BerkeleyDBKeyComparison,
                    CacheSize = new CacheInfo(10, 0, 1),
                    PageSize = 65536,
                    NoMMap = true,
                    Creation = CreatePolicy.IF_NEEDED
                };

                string databasePath = null;
                if (!m_settings.InMemoryBerkeleyDB)
                    databasePath = Path.Combine(settings.HistorianArchive, settings.HistorianName);

                m_berkeleyDb = BTreeDatabase.Open(databasePath, m_berkeleyDbCfg);
                
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

        public HistorianIArchive HistorianArchive
        {
            get
            {
                return m_historianArchive;
            }
        }

        public BTreeDatabase BerkeleyDBDatabase
        {
            get
            {
                return m_berkeleyDb;
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
                        {
                            m_berkeleyDb.Dispose();
                            Parallel.For(0, m_berkeleyDbPointList.Length, (i) =>
                            {
                                m_berkeleyDbPointList[i].Key.Dispose();
                                m_berkeleyDbPointList[i].Value.Dispose();
                            });
                        }
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
                    m_historianKey.Timestamp = point.Timestamp;
                    m_historianKey.PointID = point.PointID;

                    m_historianValue.Value1 = point.Value;
                    m_historianValue.Value3 = point.Flags;

                    m_historianArchive.Write(m_historianKey, m_historianValue);
                }
            }

            if (m_settings.WriteToBerkeleyDB) // Write to Berkeley DB
            {
                //// Create a new pointList each time *slow
                //KeyValuePair<DatabaseEntry, DatabaseEntry>[] pointList = new KeyValuePair<DatabaseEntry, DatabaseEntry>[dataBlock.Length];

                //Parallel.For(0, pointList.Length, (i) => pointList[i] = new KeyValuePair<DatabaseEntry, DatabaseEntry>(
                //                                                            new DatabaseEntry(BitConverter.GetBytes(timestamp.Ticks).Concat(BitConverter.GetBytes(dataBlock[i].PointID)).ToArray()),
                //                                                            new DatabaseEntry(BitConverter.GetBytes(dataBlock[i].Value))));

                //using (MultipleKeyDatabaseEntry buffer = new MultipleKeyDatabaseEntry(pointList, false))
                //{
                //    m_berkeleyDb.Put(buffer);
                //}

                //Parallel.For(0, pointList.Length, (i) => 
                //{
                //    pointList[i].Key.Dispose();
                //    pointList[i].Value.Dispose();
                //});


                //// Single writes using the same key and value each time *medium speed
                //foreach (DataPoint point in dataBlock)
                //{
                //    m_berkeleyDbKey.Data = BitConverter.GetBytes(timestamp.Ticks).Concat(BitConverter.GetBytes(point.PointID)).ToArray();
                //    m_berkeleyDbValue.Data = BitConverter.GetBytes(point.Value).ToArray();

                //    m_berkeleyDb.Put(m_berkeleyDbKey, m_berkeleyDbValue);
                //}

                // Bulk writes using the same pointList *fastest method found so far*
                Parallel.For(0, dataBlock.Length, (i) =>
                {
                    m_berkeleyDbPointList[i].Key.Data = BitConverter.GetBytes(timestamp.Ticks).Concat(BitConverter.GetBytes(dataBlock[i].PointID)).ToArray();
                    m_berkeleyDbPointList[i].Value.Data = BitConverter.GetBytes(dataBlock[i].Value);
                });

                using (MultipleKeyDatabaseEntry buffer = new MultipleKeyDatabaseEntry(m_berkeleyDbPointList.Take(dataBlock.Length), false))
                {
                    m_berkeleyDb.Put(buffer);
                }
            }
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
