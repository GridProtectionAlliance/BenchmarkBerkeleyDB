//******************************************************************************************************
//  BenchmarkBerkeleyDB.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  04/20/2017 - J. Ritchie Carroll
//       Generated original version of source code.
//
//  07/10/2017 - Stephen Jenks
//       Converted to BenchmarkBerkeleyDB
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GSF;
using GSF.ComponentModel;
using GSF.Diagnostics;
using GSF.IO;
using GSF.TimeSeries;
using GSF.TimeSeries.Adapters;
using GSF.Windows.Forms;
using BenchmarkBerkeleyDB.HistorianAPI;
using BenchmarkBerkeleyDB.HistorianAPI.Metadata;
using System.IO;

namespace BenchmarkBerkeleyDB
{
    /// <summary>
    /// Defines the main form for the Historian Data Walker application.
    /// </summary>
    public partial class BenchmarkBerkeleyDB : Form
    {
        #region [ Members ]


        // Nested Types
        public enum DataSource
        {
            openHistorian,
            CsvFile
        }

        public enum DestinationHistorian
        {
            openHistorian,
            BerkeleyDB
        }

        // Fields
        private readonly LogPublisher m_log;
        private Settings m_settings;
        private bool m_formClosing;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="BenchmarkBerkeleyDB"/>.
        /// </summary>
        public BenchmarkBerkeleyDB()
        {
            InitializeComponent();

            // Create a new log publisher instance
            m_log = Logger.CreatePublisher(typeof(BenchmarkBerkeleyDB), MessageClass.Application);
        }

        #endregion

        #region [ Methods ]

        // Form Event Handlers

        private void BenchmarkBerkeleyDB_Load(object sender, EventArgs e)
        {
            try
            {
                // Load current settings registering a symbolic reference to this form instance for use by default value expressions
                RadioButton test = this.groupBoxSource.Controls.OfType<RadioButton>().FirstOrDefault(x => x.Checked);

                string testTag = (string)test.Tag;
                DataSource source = (DataSource)Enum.Parse(typeof(DataSource), testTag);

                DataSource source2 = (DataSource)Enum.Parse(typeof(DataSource), (string)this.groupBoxSource.Controls.OfType<RadioButton>().FirstOrDefault(x => x.Checked).Tag);
                bool generic = typeof(DataSource).IsGenericType;
                m_settings = new Settings(new Dictionary<string, object> { { "Form", this } }.RegisterSymbols());

                // Restore last window size/location
                this.RestoreLayout();
            }
            catch (Exception ex)
            {
                m_log.Publish(MessageLevel.Error, "FormLoad", "Failed while loading settings", exception: ex);
            }
        }

        private void BenchmarkBerkeleyDB_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_formClosing = true;

                // Save current window size/location
                this.SaveLayout();

                // Save any updates to current screen values
                m_settings.Save();
            }
            catch (Exception ex)
            {
                m_log.Publish(MessageLevel.Error, "FormClosing", "Failed while saving settings", exception: ex);
            }
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            SetGoButtonEnabledState(false);
            ClearUpdateMessages();
            UpdateProgressBar(0);
            SetProgressBarMaximum(100);

            // Kick off a thread to start archive read passing in current config file settings
            new Thread(ReadArchive) { IsBackground = true }.Start();
        }

        private void buttonBrowseHistorianArchive_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxHistorianArchive.Text = dialog.SelectedPath;
            }
        }

        private void buttonBrowseCsvSource_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxCsvSource.Text = dialog.FileName;
            }
        }

        // Form Element Accessors -- these functions allow access to form elements from non-UI threads

        private void FormElementChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, EventArgs>(FormElementChanged), sender, e);
            }
            else
            {
                if (Visible)
                    m_settings?.UpdateProperties();
            }

            groupBoxSourceHistorian.Enabled = radioButtonSourceHistorian.Checked;
            groupBoxHistorianSettings.Enabled = radioButtonSourceHistorian.Checked;
            groupBoxCsvSettings.Enabled = radioButtonSourceCsv.Checked;

            textBoxHistorianArchive.Enabled = (radioButtonDestinationBerkeley.Checked && !checkBoxInMemoryBDB.Checked) || radioButtonDestinationHistorian.Checked;
            textBoxHistorianName.Enabled = radioButtonDestinationBerkeley.Checked && !checkBoxInMemoryBDB.Checked;
            buttonBrowseHistorianArchive.Enabled = (radioButtonDestinationBerkeley.Checked && !checkBoxInMemoryBDB.Checked) || radioButtonDestinationHistorian.Checked;
            maskedTextBoxDestinationHistorianDataPort.Enabled = radioButtonDestinationHistorian.Checked;
            checkBoxInMemoryBDB.Enabled = radioButtonDestinationBerkeley.Checked;
        }

        private void ShowUpdateMessage(string message)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ShowUpdateMessage), message);
            }
            else
            {
                StringBuilder outputText = new StringBuilder();

                outputText.AppendLine(message);
                outputText.AppendLine();

                lock (textBoxMessageOutput)
                    textBoxMessageOutput.AppendText(outputText.ToString());

                m_log.Publish(MessageLevel.Info, "StatusMessage", message);
            }
        }

        private void ClearUpdateMessages()
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(ClearUpdateMessages));
            }
            else
            {
                lock (textBoxMessageOutput)
                    textBoxMessageOutput.Text = "";
            }
        }

        private void SetGoButtonEnabledState(bool enabled)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
                BeginInvoke(new Action<bool>(SetGoButtonEnabledState), enabled);
            else
                buttonGo.Enabled = enabled;
        }

        private void UpdateProgressBar(int value)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(UpdateProgressBar), value);
            }
            else
            {
                if (value < progressBar.Minimum)
                    value = progressBar.Minimum;

                if (value > progressBar.Maximum)
                    progressBar.Maximum = value;

                progressBar.Value = value;
            }
        }

        private void SetProgressBarMaximum(int maximum)
        {
            if (m_formClosing)
                return;

            if (InvokeRequired)
                BeginInvoke(new Action<int>(SetProgressBarMaximum), maximum);
            else
                progressBar.Maximum = maximum;
        }

        // Internal Functions

        private void ReadArchive(object state)
        {
            double timeRange = (m_settings.EndTime - m_settings.StartTime).TotalSeconds;
            long receivedPoints = 0;
            long processedDataBlocks = 0;
            Ticks totalRetrievalTime = 0;
            Ticks totalWriteTime = 0;
            Ticks totalReadBackTime = 0;
            Ticks largeOperationTimer;
            Ticks smallOperationTimer;

            using (DataReader dataReader = new DataReader(m_settings, ShowUpdateMessage, m_log))
            using (DataWriter dataWriter = new DataWriter(m_settings, dataReader.PointCount))
            {
                DataPoint[] points = new DataPoint[dataReader.PointCount];
                dataWriter.ShowMessage = ShowUpdateMessage;
                ShowUpdateMessage(">>> Starting archive read...");

                // Start historian data read
                largeOperationTimer = DateTime.UtcNow;
                while (!m_formClosing)
                {
                    // Time reading operation
                    smallOperationTimer = DateTime.UtcNow;
                    if (!dataReader.Read(out points))
                        break;
                    totalRetrievalTime += (DateTime.UtcNow.Ticks - smallOperationTimer);

                    receivedPoints += points.Length;
                    if (++processedDataBlocks % m_settings.MessageInterval == 0)
                    {
                        ShowUpdateMessage($"{Environment.NewLine}{receivedPoints:N0} points processed so far averaging {receivedPoints / (DateTime.UtcNow.Ticks - largeOperationTimer).ToSeconds():N0} points per second.");
                        UpdateProgressBar(dataReader.PercentComplete);
                    }

                    try
                    {
                        // Time writing operation
                        smallOperationTimer = DateTime.UtcNow;
                        dataWriter.Write(dataReader.CurrentTimestamp, points); // Write data block
                        totalWriteTime += DateTime.UtcNow.Ticks - smallOperationTimer;
                    }
                    catch (Exception ex)
                    {
                        ShowUpdateMessage($"ERROR: DataWriter exception: {ex.Message}");
                        m_log.Publish(MessageLevel.Error, "DataWriterError", "Failed while processing data", exception: ex);
                    }
                }

                if (m_formClosing)
                {
                    ShowUpdateMessage("*** Historian Read Canceled ***");
                    UpdateProgressBar(0);
                }
                else
                {
                    ShowUpdateMessage("*** Historian Read Complete ***");
                    UpdateProgressBar(100);

                    if (m_settings.ReadBack)
                    {
                        if (m_settings.WriteToOpenHistorian)
                            totalReadBackTime = dataReader.ReadBackHistorianData(dataWriter.HistorianArchive);
                        if (m_settings.WriteToBerkeleyDB)
                            totalReadBackTime = dataReader.ReadBackBerkeleyDBData(dataWriter.BerkeleyDBDatabase);
                    }

                    DisplayStats(totalRetrievalTime, totalWriteTime, totalReadBackTime, receivedPoints);

                }

            }

            SetGoButtonEnabledState(true);
        }

        private void DisplayStats(Ticks readTime, Ticks writeTime, Ticks readBackTime, long receivedPoints)
        {
            ShowUpdateMessage($"Total time spent reading: {readTime.ToSeconds()} seconds{(readTime.ToSeconds() != 0 ? $", averaging {receivedPoints / readTime.ToSeconds():N0} points per second" : "")}");
            ShowUpdateMessage($"Total time spent writing: {writeTime.ToSeconds()} seconds{(writeTime.ToSeconds() != 0 ? $", averaging {receivedPoints / writeTime.ToSeconds():N0} points per second" : "")}");
            ShowUpdateMessage($"Total time spent reading back data: {readBackTime.ToSeconds()} seconds{(readBackTime.ToSeconds() != 0 ? $", averaging {receivedPoints / readBackTime.ToSeconds():N0} points per second" : "")}");
        }

        #endregion

        #region [ Static ]

        private static string GetRootTagName(string tagName)
        {
            int lastBangIndex = tagName.LastIndexOf('!');
            return lastBangIndex > -1 ? tagName.Substring(lastBangIndex + 1).Trim() : tagName.Trim();
        }

        // Static Constructor
        static BenchmarkBerkeleyDB()
        {
            // Set default logging path
            Logger.FileWriter.SetPath(FilePath.GetAbsolutePath(""), VerboseLevel.Ultra);
        }

        #endregion
    }
}