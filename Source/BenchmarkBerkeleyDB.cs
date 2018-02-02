﻿//******************************************************************************************************
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

namespace BenchmarkBerkeleyDB
{
    /// <summary>
    /// Defines the main form for the Historian Data Walker application.
    /// </summary>
    public partial class BenchmarkBerkeleyDB : Form
    {
        #region [ Members ]

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

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxDestination.Text = dialog.SelectedPath;
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
            try
            {
                DateTime startTime = new DateTime(m_settings.StartTime.Ticks, m_settings.UseUTCTime ? DateTimeKind.Utc : DateTimeKind.Local);
                DateTime endTime = new DateTime(m_settings.EndTime.Ticks, m_settings.UseUTCTime ? DateTimeKind.Utc : DateTimeKind.Local);

                // Convert all times to UTC since that's what the openHistorian uses.
                startTime = startTime.ToUniversalTime();
                endTime = endTime.ToUniversalTime();

                double timeRange = (endTime - startTime).TotalSeconds;
                long receivedPoints = 0;
                long processedDataBlocks = 0;
                long duplicatePoints = 0;
                Ticks operationTime;
                Ticks operationStartTime;
                DataPoint point = new DataPoint();

                // Load historian meta-data
                ShowUpdateMessage(">>> Loading source connection metadata...");

                operationStartTime = DateTime.UtcNow.Ticks;
                List<MetadataRecord> metadata = MetadataRecord.Query(m_settings.HostAddress, m_settings.MetadataPort, m_settings.MetadataTimeout);
                operationTime = DateTime.UtcNow.Ticks - operationStartTime;

                ShowUpdateMessage("*** Metadata Load Complete ***");
                ShowUpdateMessage($"Total metadata load time {operationTime.ToElapsedTimeString(3)}...");

                // Parse meta-data expression
                ShowUpdateMessage(">>> Processing filter expression for metadata...");
                operationStartTime = DateTime.UtcNow.Ticks;
                MeasurementKey[] inputKeys = AdapterBase.ParseInputMeasurementKeys(MetadataRecord.Metadata, false, textBoxPointList.Text, "MeasurementDetail");
                List<ulong> pointIDList = inputKeys.Select(key => (ulong)key.ID).ToList();
                List<MetadataRecord> records = new List<MetadataRecord>();

                foreach (ulong pointID in pointIDList)
                {
                    MetadataRecord record = metadata.FirstOrDefault(md => md.PointID == pointID);

                    if ((object)record != null)
                        records.Add(record);
                }

                operationTime = DateTime.UtcNow.Ticks - operationStartTime;

                ShowUpdateMessage($">>> Historian read will be for {pointIDList.Count:N0} points based on provided meta-data expression.");

                ShowUpdateMessage("*** Filter Expression Processing Complete ***");
                ShowUpdateMessage($"Total filter expression processing time {operationTime.ToElapsedTimeString(3)}...");

                using (Algorithm algorithm = new Algorithm(records))
                {
                    algorithm.ShowMessage = ShowUpdateMessage;
                    algorithm.MessageInterval = m_settings.MessageInterval;
                    algorithm.Log = m_log;

                    ShowUpdateMessage(">>> Starting archive read...");

                    // Start historian data read
                    operationStartTime = DateTime.UtcNow.Ticks;

                    using (SnapDBClient historianClient = new SnapDBClient(m_settings.HostAddress, m_settings.DataPort, m_settings.InstanceName, startTime, endTime, m_settings.FrameRate, pointIDList))
                    {
                        // Scan to first record
                        if (!historianClient.ReadNext(point))
                            throw new InvalidOperationException("No data for specified time range in openHistorian connection!");

                        ulong currentTimestamp;
                        receivedPoints++;

                        while (!m_formClosing)
                        {
                            int timeComparison;
                            bool readSuccess = true;

                            // Create a new data block for current timestamp and load first/prior point
                            Dictionary<ulong, DataPoint> dataBlock = new Dictionary<ulong, DataPoint>
                            {
                                [point.PointID] = point.Clone()
                            };

                            currentTimestamp = point.Timestamp;

                            // Load remaining data for current timestamp
                            do
                            {
                                // Scan to next record
                                if (!historianClient.ReadNext(point))
                                {
                                    readSuccess = false;
                                    break;
                                }

                                receivedPoints++;
                                timeComparison = DataPoint.CompareTimestamps(point.Timestamp, currentTimestamp, m_settings.FrameRate);

                                if (timeComparison == 0)
                                {
                                    // Timestamps are compared based on configured frame rate - if archived data rate is
                                    // higher than configured frame rate, then data block will contain only latest values
                                    if (dataBlock.ContainsKey(point.PointID))
                                        duplicatePoints++;

                                    dataBlock[point.PointID] = point.Clone();
                                }
                            }
                            while (timeComparison == 0);

                            // Finished with data read
                            if (!readSuccess)
                            {
                                ShowUpdateMessage(">>> End of data in range encountered...");
                                break;
                            }

                            if (++processedDataBlocks % m_settings.MessageInterval == 0)
                            {
                                ShowUpdateMessage($"{Environment.NewLine}{receivedPoints:N0} points{(duplicatePoints > 0 ? $", which included {duplicatePoints:N0} duplicates," : "")} read so far averaging {receivedPoints / (DateTime.UtcNow.Ticks - operationStartTime).ToSeconds():N0} points per second.");
                                UpdateProgressBar((int)((1.0D - new Ticks(endTime.Ticks - (long)point.Timestamp).ToSeconds() / timeRange) * 100.0D));
                            }

                            try
                            {
                                // Analyze data block
                                algorithm.Execute(new DateTime((long)currentTimestamp), dataBlock.Values.ToArray());
                            }
                            catch (Exception ex)
                            {
                                ShowUpdateMessage($"ERROR: Algorithm exception: {ex.Message}");
                                m_log.Publish(MessageLevel.Error, "AlgorithmError", "Failed while processing data from the historian", exception: ex);
                            }
                        }

                        operationTime = DateTime.UtcNow.Ticks - operationStartTime;

                        if (m_formClosing)
                        {
                            ShowUpdateMessage("*** Historian Read Canceled ***");
                            UpdateProgressBar(0);
                        }
                        else
                        {
                            ShowUpdateMessage("*** Historian Read Complete ***");
                            UpdateProgressBar(100);
                        }

                        algorithm.ReadDb();

                        // Show some operational statistics
                        long expectedPoints = (long)(timeRange * m_settings.FrameRate * algorithm.Metadata.Count);
                        double dataCompleteness = Math.Round(receivedPoints / (double)expectedPoints * 100000.0D) / 1000.0D;

                        string overallSummary =
                            $"Total read time {operationTime.ToElapsedTimeString(3)} at {receivedPoints / operationTime.ToSeconds():N0} points per second.{Environment.NewLine}" +
                            $"{Environment.NewLine}" +
                            $"           Meta-data points: {algorithm.Metadata.Count}{Environment.NewLine}" +
                            $"          Time-span covered: {timeRange:N0} seconds: {Ticks.FromSeconds(timeRange).ToElapsedTimeString(2)}{Environment.NewLine}" +
                            $"       Processed timestamps: {processedDataBlocks:N0}{Environment.NewLine}" +
                            $"            Expected points: {expectedPoints:N0} @ {m_settings.FrameRate:N0} samples per second{Environment.NewLine}" +
                            $"            Received points: {receivedPoints:N0}{Environment.NewLine}" +
                            $"           Duplicate points: {duplicatePoints:N0}{Environment.NewLine}" +
                            $"          Data completeness: {dataCompleteness:N3}%{Environment.NewLine}";

                        ShowUpdateMessage(overallSummary);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowUpdateMessage($"!!! Failure during historian read: {ex.Message}");
                m_log.Publish(MessageLevel.Error, "HistorianDataRead", "Failed while reading data from the historian", exception: ex);
            }
            finally
            {
                SetGoButtonEnabledState(true);
            }
        }

        private static string GetRootTagName(string tagName)
        {
            int lastBangIndex = tagName.LastIndexOf('!');
            return lastBangIndex > -1 ? tagName.Substring(lastBangIndex + 1).Trim() : tagName.Trim();
        }

        #endregion

        #region [ Static ]

        // Static Constructor
        static BenchmarkBerkeleyDB()
        {
            // Set default logging path
            Logger.FileWriter.SetPath(FilePath.GetAbsolutePath(""), VerboseLevel.Ultra);
        }

        #endregion
    }
}