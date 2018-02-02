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

namespace BenchmarkBerkeleyDB
{
    /// <summary>
    /// Defines algorithm to be executed during historian read.
    /// </summary>
    public class Algorithm : IDisposable
    {
        #region [ Members ]

        // Nested Types

        // Meta-data fields
        private List<MetadataRecord> m_metadata;

        // Algorithm analytic fields
        private BTreeDatabaseConfig m_cfg;
        private BTreeDatabase m_db;

        // Algorithm processing statistic fields
        private long m_processedDataBlocks;

        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        public Algorithm(List<MetadataRecord> metadata)
        {
            m_cfg = new BTreeDatabaseConfig()
            {
                BTreeCompare = KeyComparison,
                Creation = CreatePolicy.IF_NEEDED
            };
            m_db = BTreeDatabase.Open("Berkeley.db", m_cfg);

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
        /// Releases all the resources used by the <see cref="Algorithm"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Algorithm"/> object and optionally releases the managed resources.
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
                        m_db.Close();
                    }
                }
                finally
                {
                    m_disposed = true;  // Prevent duplicate dispose.
                }
            }
        }

        /// <summary>
        /// Default data processing entry point for <see cref="Algorithm"/>.
        /// </summary>
        /// <param name="timestamp">Timestamp of <paramref name="dataBlock"/>.</param>
        /// <param name="dataBlock">Points values read at current timestamp.</param>
        public void Execute(DateTime timestamp, DataPoint[] dataBlock)
        {
            KeyValuePair<DatabaseEntry, DatabaseEntry>[] pointList = new KeyValuePair<DatabaseEntry, DatabaseEntry>[dataBlock.Length];

            Parallel.For(0, pointList.Length, (i) => pointList[i] = new KeyValuePair<DatabaseEntry, DatabaseEntry>(
                                                                        new DatabaseEntry(BitConverter.GetBytes(timestamp.Ticks).Concat(BitConverter.GetBytes(dataBlock[i].PointID)).ToArray()),
                                                                        new DatabaseEntry(BitConverter.GetBytes(dataBlock[i].ValueAsSingle))));

            m_db.Put(new MultipleKeyDatabaseEntry(pointList, false));
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

            cursor = m_db.Cursor();

            while (cursor.MoveNextMultipleKey())
            {
                MultipleKeyDatabaseEntry pairs = cursor.CurrentMultipleKey;
                
                foreach (KeyValuePair<DatabaseEntry, DatabaseEntry> p in pairs)
                {
                    value = BitConverter.ToSingle(p.Value.Data, 0);
                    count++;
                }
            }

            double seconds = (DateTime.Now - startTime).TotalSeconds;

            ShowMessage($"{count} points processed in {seconds} seconds. Averaging {(int)Math.Round(count / seconds)} points per second.");
        }

        int KeyComparison(DatabaseEntry key1, DatabaseEntry key2)
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
