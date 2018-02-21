namespace BenchmarkBerkeleyDB
{
    partial class BenchmarkBerkeleyDB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BenchmarkBerkeleyDB));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBoxMessages = new System.Windows.Forms.GroupBox();
            this.textBoxMessageOutput = new System.Windows.Forms.TextBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.groupBoxHistorianSettings = new System.Windows.Forms.GroupBox();
            this.checkBoxUseUTCTime = new System.Windows.Forms.CheckBox();
            this.labelPointList = new System.Windows.Forms.Label();
            this.textBoxPointList = new System.Windows.Forms.TextBox();
            this.labelSeconds = new System.Windows.Forms.Label();
            this.labelPerSec = new System.Windows.Forms.Label();
            this.maskedTextBoxMetadataTimeout = new System.Windows.Forms.MaskedTextBox();
            this.labelFrameRate = new System.Windows.Forms.Label();
            this.labelMetaDataTimeout = new System.Windows.Forms.Label();
            this.maskedTextBoxFrameRate = new System.Windows.Forms.MaskedTextBox();
            this.labelEndTime = new System.Windows.Forms.Label();
            this.dateTimePickerEndTime = new System.Windows.Forms.DateTimePicker();
            this.labelStartTime = new System.Windows.Forms.Label();
            this.dateTimePickerSourceTime = new System.Windows.Forms.DateTimePicker();
            this.buttonBrowseHistorianArchive = new System.Windows.Forms.Button();
            this.textBoxHistorianArchive = new System.Windows.Forms.TextBox();
            this.labelDestinationDirectory = new System.Windows.Forms.Label();
            this.checkBoxEnableLogging = new System.Windows.Forms.CheckBox();
            this.maskedTextBoxMessageInterval = new System.Windows.Forms.MaskedTextBox();
            this.labelMessageInterval = new System.Windows.Forms.Label();
            this.groupBoxCsvSettings = new System.Windows.Forms.GroupBox();
            this.buttonBrowseCsvSource = new System.Windows.Forms.Button();
            this.textBoxCsvSource = new System.Windows.Forms.TextBox();
            this.labelSourceFile = new System.Windows.Forms.Label();
            this.groupBoxSource = new System.Windows.Forms.GroupBox();
            this.radioButtonSourceCsv = new System.Windows.Forms.RadioButton();
            this.radioButtonSourceHistorian = new System.Windows.Forms.RadioButton();
            this.groupBoxDestination = new System.Windows.Forms.GroupBox();
            this.radioButtonDestinationBerkeley = new System.Windows.Forms.RadioButton();
            this.radioButtonDestinationHistorian = new System.Windows.Forms.RadioButton();
            this.labelHistorianName = new System.Windows.Forms.Label();
            this.textBoxHistorianName = new System.Windows.Forms.TextBox();
            this.labelSourceHistorianMetaDataPort = new System.Windows.Forms.Label();
            this.labelSourceHistorianDataPort = new System.Windows.Forms.Label();
            this.maskedTextBoxHistorianMetadataPort = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxHistorianDataPort = new System.Windows.Forms.MaskedTextBox();
            this.textBoxHistorianHostAddress = new System.Windows.Forms.TextBox();
            this.labelSourceHistorianHostAddress = new System.Windows.Forms.Label();
            this.textBoxHistorianInstanceName = new System.Windows.Forms.TextBox();
            this.labelSourceHistorianInstanceName = new System.Windows.Forms.Label();
            this.groupBoxSourceHistorian = new System.Windows.Forms.GroupBox();
            this.groupBoxGeneralSettings = new System.Windows.Forms.GroupBox();
            this.groupBoxMessages.SuspendLayout();
            this.groupBoxHistorianSettings.SuspendLayout();
            this.groupBoxCsvSettings.SuspendLayout();
            this.groupBoxSource.SuspendLayout();
            this.groupBoxDestination.SuspendLayout();
            this.groupBoxSourceHistorian.SuspendLayout();
            this.groupBoxGeneralSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(8, 677);
            this.progressBar.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(598, 27);
            this.progressBar.TabIndex = 3;
            // 
            // groupBoxMessages
            // 
            this.groupBoxMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMessages.Controls.Add(this.textBoxMessageOutput);
            this.groupBoxMessages.Location = new System.Drawing.Point(8, 377);
            this.groupBoxMessages.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxMessages.Name = "groupBoxMessages";
            this.groupBoxMessages.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxMessages.Size = new System.Drawing.Size(645, 296);
            this.groupBoxMessages.TabIndex = 2;
            this.groupBoxMessages.TabStop = false;
            this.groupBoxMessages.Text = "Messages";
            // 
            // textBoxMessageOutput
            // 
            this.textBoxMessageOutput.BackColor = System.Drawing.SystemColors.WindowText;
            this.textBoxMessageOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessageOutput.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessageOutput.ForeColor = System.Drawing.SystemColors.Window;
            this.textBoxMessageOutput.Location = new System.Drawing.Point(2, 15);
            this.textBoxMessageOutput.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMessageOutput.Multiline = true;
            this.textBoxMessageOutput.Name = "textBoxMessageOutput";
            this.textBoxMessageOutput.ReadOnly = true;
            this.textBoxMessageOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessageOutput.Size = new System.Drawing.Size(641, 279);
            this.textBoxMessageOutput.TabIndex = 0;
            this.textBoxMessageOutput.TabStop = false;
            // 
            // buttonGo
            // 
            this.buttonGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGo.Location = new System.Drawing.Point(603, 677);
            this.buttonGo.Margin = new System.Windows.Forms.Padding(2);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(50, 27);
            this.buttonGo.TabIndex = 4;
            this.buttonGo.Text = "&Go!";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // groupBoxHistorianSettings
            // 
            this.groupBoxHistorianSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxHistorianSettings.Controls.Add(this.checkBoxUseUTCTime);
            this.groupBoxHistorianSettings.Controls.Add(this.labelPointList);
            this.groupBoxHistorianSettings.Controls.Add(this.textBoxPointList);
            this.groupBoxHistorianSettings.Controls.Add(this.labelSeconds);
            this.groupBoxHistorianSettings.Controls.Add(this.labelPerSec);
            this.groupBoxHistorianSettings.Controls.Add(this.maskedTextBoxMetadataTimeout);
            this.groupBoxHistorianSettings.Controls.Add(this.labelFrameRate);
            this.groupBoxHistorianSettings.Controls.Add(this.labelMetaDataTimeout);
            this.groupBoxHistorianSettings.Controls.Add(this.maskedTextBoxFrameRate);
            this.groupBoxHistorianSettings.Controls.Add(this.labelEndTime);
            this.groupBoxHistorianSettings.Controls.Add(this.dateTimePickerEndTime);
            this.groupBoxHistorianSettings.Controls.Add(this.labelStartTime);
            this.groupBoxHistorianSettings.Controls.Add(this.dateTimePickerSourceTime);
            this.groupBoxHistorianSettings.Location = new System.Drawing.Point(206, 8);
            this.groupBoxHistorianSettings.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxHistorianSettings.Name = "groupBoxHistorianSettings";
            this.groupBoxHistorianSettings.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxHistorianSettings.Size = new System.Drawing.Size(447, 154);
            this.groupBoxHistorianSettings.TabIndex = 1;
            this.groupBoxHistorianSettings.TabStop = false;
            this.groupBoxHistorianSettings.Text = "&Historian Settings";
            // 
            // checkBoxUseUTCTime
            // 
            this.checkBoxUseUTCTime.AutoSize = true;
            this.checkBoxUseUTCTime.Location = new System.Drawing.Point(222, 49);
            this.checkBoxUseUTCTime.Name = "checkBoxUseUTCTime";
            this.checkBoxUseUTCTime.Size = new System.Drawing.Size(96, 17);
            this.checkBoxUseUTCTime.TabIndex = 20;
            this.checkBoxUseUTCTime.Text = "Use UTC Time";
            this.checkBoxUseUTCTime.UseVisualStyleBackColor = true;
            this.checkBoxUseUTCTime.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelPointList
            // 
            this.labelPointList.AutoSize = true;
            this.labelPointList.Location = new System.Drawing.Point(10, 51);
            this.labelPointList.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPointList.Name = "labelPointList";
            this.labelPointList.Size = new System.Drawing.Size(140, 13);
            this.labelPointList.TabIndex = 4;
            this.labelPointList.Text = "Point List / Filter Expression:";
            this.labelPointList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPointList
            // 
            this.textBoxPointList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPointList.Location = new System.Drawing.Point(12, 67);
            this.textBoxPointList.Multiline = true;
            this.textBoxPointList.Name = "textBoxPointList";
            this.textBoxPointList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPointList.Size = new System.Drawing.Size(424, 49);
            this.textBoxPointList.TabIndex = 5;
            this.textBoxPointList.Text = "FILTER MeasurementDetail WHERE SignalAcronym IN (\'FREQ\')";
            this.textBoxPointList.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelSeconds
            // 
            this.labelSeconds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeconds.AutoSize = true;
            this.labelSeconds.Location = new System.Drawing.Point(390, 124);
            this.labelSeconds.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSeconds.Name = "labelSeconds";
            this.labelSeconds.Size = new System.Drawing.Size(47, 13);
            this.labelSeconds.TabIndex = 11;
            this.labelSeconds.Text = "seconds";
            // 
            // labelPerSec
            // 
            this.labelPerSec.AutoSize = true;
            this.labelPerSec.Location = new System.Drawing.Point(144, 124);
            this.labelPerSec.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPerSec.Name = "labelPerSec";
            this.labelPerSec.Size = new System.Drawing.Size(101, 13);
            this.labelPerSec.TabIndex = 8;
            this.labelPerSec.Text = "samples per second";
            // 
            // maskedTextBoxMetadataTimeout
            // 
            this.maskedTextBoxMetadataTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maskedTextBoxMetadataTimeout.Location = new System.Drawing.Point(355, 121);
            this.maskedTextBoxMetadataTimeout.Margin = new System.Windows.Forms.Padding(2);
            this.maskedTextBoxMetadataTimeout.Mask = "000";
            this.maskedTextBoxMetadataTimeout.Name = "maskedTextBoxMetadataTimeout";
            this.maskedTextBoxMetadataTimeout.Size = new System.Drawing.Size(31, 20);
            this.maskedTextBoxMetadataTimeout.TabIndex = 10;
            this.maskedTextBoxMetadataTimeout.Text = "60";
            this.maskedTextBoxMetadataTimeout.ValidatingType = typeof(int);
            this.maskedTextBoxMetadataTimeout.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelFrameRate
            // 
            this.labelFrameRate.AutoSize = true;
            this.labelFrameRate.Location = new System.Drawing.Point(4, 124);
            this.labelFrameRate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFrameRate.Name = "labelFrameRate";
            this.labelFrameRate.Size = new System.Drawing.Size(103, 13);
            this.labelFrameRate.TabIndex = 6;
            this.labelFrameRate.Text = "Frame-rate Estimate:";
            this.labelFrameRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMetaDataTimeout
            // 
            this.labelMetaDataTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMetaDataTimeout.AutoSize = true;
            this.labelMetaDataTimeout.Location = new System.Drawing.Point(252, 124);
            this.labelMetaDataTimeout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMetaDataTimeout.Name = "labelMetaDataTimeout";
            this.labelMetaDataTimeout.Size = new System.Drawing.Size(99, 13);
            this.labelMetaDataTimeout.TabIndex = 9;
            this.labelMetaDataTimeout.Text = "Meta-data Timeout:";
            this.labelMetaDataTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskedTextBoxFrameRate
            // 
            this.maskedTextBoxFrameRate.Location = new System.Drawing.Point(110, 121);
            this.maskedTextBoxFrameRate.Margin = new System.Windows.Forms.Padding(2);
            this.maskedTextBoxFrameRate.Mask = "000";
            this.maskedTextBoxFrameRate.Name = "maskedTextBoxFrameRate";
            this.maskedTextBoxFrameRate.Size = new System.Drawing.Size(31, 20);
            this.maskedTextBoxFrameRate.TabIndex = 7;
            this.maskedTextBoxFrameRate.Text = "30";
            this.maskedTextBoxFrameRate.ValidatingType = typeof(int);
            this.maskedTextBoxFrameRate.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelEndTime
            // 
            this.labelEndTime.AutoSize = true;
            this.labelEndTime.Location = new System.Drawing.Point(227, 27);
            this.labelEndTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEndTime.Name = "labelEndTime";
            this.labelEndTime.Size = new System.Drawing.Size(55, 13);
            this.labelEndTime.TabIndex = 2;
            this.labelEndTime.Text = "End Time:";
            this.labelEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerEndTime
            // 
            this.dateTimePickerEndTime.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dateTimePickerEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEndTime.Location = new System.Drawing.Point(285, 24);
            this.dateTimePickerEndTime.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerEndTime.Name = "dateTimePickerEndTime";
            this.dateTimePickerEndTime.Size = new System.Drawing.Size(147, 20);
            this.dateTimePickerEndTime.TabIndex = 3;
            this.dateTimePickerEndTime.Value = new System.DateTime(2017, 1, 1, 0, 10, 0, 0);
            this.dateTimePickerEndTime.ValueChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelStartTime
            // 
            this.labelStartTime.AutoSize = true;
            this.labelStartTime.Location = new System.Drawing.Point(10, 27);
            this.labelStartTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStartTime.Name = "labelStartTime";
            this.labelStartTime.Size = new System.Drawing.Size(58, 13);
            this.labelStartTime.TabIndex = 0;
            this.labelStartTime.Text = "Start Time:";
            this.labelStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerSourceTime
            // 
            this.dateTimePickerSourceTime.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dateTimePickerSourceTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerSourceTime.Location = new System.Drawing.Point(72, 24);
            this.dateTimePickerSourceTime.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerSourceTime.Name = "dateTimePickerSourceTime";
            this.dateTimePickerSourceTime.Size = new System.Drawing.Size(147, 20);
            this.dateTimePickerSourceTime.TabIndex = 1;
            this.dateTimePickerSourceTime.Value = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerSourceTime.ValueChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // buttonBrowseHistorianArchive
            // 
            this.buttonBrowseHistorianArchive.Location = new System.Drawing.Point(428, 18);
            this.buttonBrowseHistorianArchive.Name = "buttonBrowseHistorianArchive";
            this.buttonBrowseHistorianArchive.Size = new System.Drawing.Size(52, 23);
            this.buttonBrowseHistorianArchive.TabIndex = 17;
            this.buttonBrowseHistorianArchive.Text = "Browse";
            this.buttonBrowseHistorianArchive.UseVisualStyleBackColor = true;
            this.buttonBrowseHistorianArchive.Click += new System.EventHandler(this.buttonBrowseHistorianArchive_Click);
            // 
            // textBoxHistorianArchive
            // 
            this.textBoxHistorianArchive.Location = new System.Drawing.Point(64, 19);
            this.textBoxHistorianArchive.Name = "textBoxHistorianArchive";
            this.textBoxHistorianArchive.Size = new System.Drawing.Size(353, 20);
            this.textBoxHistorianArchive.TabIndex = 16;
            this.textBoxHistorianArchive.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelDestinationDirectory
            // 
            this.labelDestinationDirectory.AutoSize = true;
            this.labelDestinationDirectory.Location = new System.Drawing.Point(7, 23);
            this.labelDestinationDirectory.Name = "labelDestinationDirectory";
            this.labelDestinationDirectory.Size = new System.Drawing.Size(52, 13);
            this.labelDestinationDirectory.TabIndex = 15;
            this.labelDestinationDirectory.Text = "Directory:";
            // 
            // checkBoxEnableLogging
            // 
            this.checkBoxEnableLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxEnableLogging.AutoSize = true;
            this.checkBoxEnableLogging.Checked = true;
            this.checkBoxEnableLogging.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEnableLogging.Location = new System.Drawing.Point(14, 63);
            this.checkBoxEnableLogging.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxEnableLogging.Name = "checkBoxEnableLogging";
            this.checkBoxEnableLogging.Size = new System.Drawing.Size(100, 17);
            this.checkBoxEnableLogging.TabIndex = 14;
            this.checkBoxEnableLogging.Text = "Enable Logging";
            this.checkBoxEnableLogging.UseVisualStyleBackColor = true;
            this.checkBoxEnableLogging.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // maskedTextBoxMessageInterval
            // 
            this.maskedTextBoxMessageInterval.Location = new System.Drawing.Point(105, 21);
            this.maskedTextBoxMessageInterval.Margin = new System.Windows.Forms.Padding(2);
            this.maskedTextBoxMessageInterval.Mask = "0000000";
            this.maskedTextBoxMessageInterval.Name = "maskedTextBoxMessageInterval";
            this.maskedTextBoxMessageInterval.Size = new System.Drawing.Size(53, 20);
            this.maskedTextBoxMessageInterval.TabIndex = 13;
            this.maskedTextBoxMessageInterval.Text = "2000";
            this.maskedTextBoxMessageInterval.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelMessageInterval
            // 
            this.labelMessageInterval.AutoSize = true;
            this.labelMessageInterval.Location = new System.Drawing.Point(11, 23);
            this.labelMessageInterval.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMessageInterval.Name = "labelMessageInterval";
            this.labelMessageInterval.Size = new System.Drawing.Size(91, 13);
            this.labelMessageInterval.TabIndex = 12;
            this.labelMessageInterval.Text = "Message Interval:";
            this.labelMessageInterval.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBoxCsvSettings
            // 
            this.groupBoxCsvSettings.Controls.Add(this.buttonBrowseCsvSource);
            this.groupBoxCsvSettings.Controls.Add(this.textBoxCsvSource);
            this.groupBoxCsvSettings.Controls.Add(this.labelSourceFile);
            this.groupBoxCsvSettings.Location = new System.Drawing.Point(206, 167);
            this.groupBoxCsvSettings.Name = "groupBoxCsvSettings";
            this.groupBoxCsvSettings.Size = new System.Drawing.Size(441, 59);
            this.groupBoxCsvSettings.TabIndex = 22;
            this.groupBoxCsvSettings.TabStop = false;
            this.groupBoxCsvSettings.Text = "CSV Settings";
            // 
            // buttonBrowseCsvSource
            // 
            this.buttonBrowseCsvSource.Location = new System.Drawing.Point(381, 18);
            this.buttonBrowseCsvSource.Name = "buttonBrowseCsvSource";
            this.buttonBrowseCsvSource.Size = new System.Drawing.Size(52, 23);
            this.buttonBrowseCsvSource.TabIndex = 20;
            this.buttonBrowseCsvSource.Text = "Browse";
            this.buttonBrowseCsvSource.UseVisualStyleBackColor = true;
            this.buttonBrowseCsvSource.Click += new System.EventHandler(this.buttonBrowseCsvSource_Click);
            // 
            // textBoxCsvSource
            // 
            this.textBoxCsvSource.Location = new System.Drawing.Point(70, 20);
            this.textBoxCsvSource.Name = "textBoxCsvSource";
            this.textBoxCsvSource.Size = new System.Drawing.Size(304, 20);
            this.textBoxCsvSource.TabIndex = 19;
            this.textBoxCsvSource.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelSourceFile
            // 
            this.labelSourceFile.AutoSize = true;
            this.labelSourceFile.Location = new System.Drawing.Point(1, 24);
            this.labelSourceFile.Name = "labelSourceFile";
            this.labelSourceFile.Size = new System.Drawing.Size(60, 13);
            this.labelSourceFile.TabIndex = 18;
            this.labelSourceFile.Text = "Source file:";
            // 
            // groupBoxSource
            // 
            this.groupBoxSource.Controls.Add(this.radioButtonSourceCsv);
            this.groupBoxSource.Controls.Add(this.radioButtonSourceHistorian);
            this.groupBoxSource.Location = new System.Drawing.Point(206, 232);
            this.groupBoxSource.Name = "groupBoxSource";
            this.groupBoxSource.Size = new System.Drawing.Size(432, 36);
            this.groupBoxSource.TabIndex = 23;
            this.groupBoxSource.TabStop = false;
            this.groupBoxSource.Text = "Source";
            // 
            // radioButtonSourceCsv
            // 
            this.radioButtonSourceCsv.AutoSize = true;
            this.radioButtonSourceCsv.Location = new System.Drawing.Point(255, 11);
            this.radioButtonSourceCsv.Name = "radioButtonSourceCsv";
            this.radioButtonSourceCsv.Size = new System.Drawing.Size(65, 17);
            this.radioButtonSourceCsv.TabIndex = 1;
            this.radioButtonSourceCsv.Tag = "CsvFile";
            this.radioButtonSourceCsv.Text = "CSV File";
            this.radioButtonSourceCsv.UseVisualStyleBackColor = true;
            this.radioButtonSourceCsv.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // radioButtonSourceHistorian
            // 
            this.radioButtonSourceHistorian.AutoSize = true;
            this.radioButtonSourceHistorian.Checked = true;
            this.radioButtonSourceHistorian.Location = new System.Drawing.Point(72, 11);
            this.radioButtonSourceHistorian.Name = "radioButtonSourceHistorian";
            this.radioButtonSourceHistorian.Size = new System.Drawing.Size(90, 17);
            this.radioButtonSourceHistorian.TabIndex = 0;
            this.radioButtonSourceHistorian.TabStop = true;
            this.radioButtonSourceHistorian.Tag = "openHistorian";
            this.radioButtonSourceHistorian.Text = "openHistorian";
            this.radioButtonSourceHistorian.UseVisualStyleBackColor = true;
            this.radioButtonSourceHistorian.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // groupBoxDestination
            // 
            this.groupBoxDestination.Controls.Add(this.radioButtonDestinationBerkeley);
            this.groupBoxDestination.Controls.Add(this.radioButtonDestinationHistorian);
            this.groupBoxDestination.Controls.Add(this.labelHistorianName);
            this.groupBoxDestination.Controls.Add(this.textBoxHistorianName);
            this.groupBoxDestination.Controls.Add(this.buttonBrowseHistorianArchive);
            this.groupBoxDestination.Controls.Add(this.labelDestinationDirectory);
            this.groupBoxDestination.Controls.Add(this.textBoxHistorianArchive);
            this.groupBoxDestination.Location = new System.Drawing.Point(8, 274);
            this.groupBoxDestination.Name = "groupBoxDestination";
            this.groupBoxDestination.Size = new System.Drawing.Size(630, 95);
            this.groupBoxDestination.TabIndex = 24;
            this.groupBoxDestination.TabStop = false;
            this.groupBoxDestination.Text = "Destination";
            // 
            // radioButtonDestinationBerkeley
            // 
            this.radioButtonDestinationBerkeley.AutoSize = true;
            this.radioButtonDestinationBerkeley.Location = new System.Drawing.Point(520, 54);
            this.radioButtonDestinationBerkeley.Name = "radioButtonDestinationBerkeley";
            this.radioButtonDestinationBerkeley.Size = new System.Drawing.Size(84, 17);
            this.radioButtonDestinationBerkeley.TabIndex = 21;
            this.radioButtonDestinationBerkeley.Tag = "BerkeleyDB";
            this.radioButtonDestinationBerkeley.Text = "Berkeley DB";
            this.radioButtonDestinationBerkeley.UseVisualStyleBackColor = true;
            this.radioButtonDestinationBerkeley.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // radioButtonDestinationHistorian
            // 
            this.radioButtonDestinationHistorian.AutoSize = true;
            this.radioButtonDestinationHistorian.Checked = true;
            this.radioButtonDestinationHistorian.Location = new System.Drawing.Point(520, 19);
            this.radioButtonDestinationHistorian.Name = "radioButtonDestinationHistorian";
            this.radioButtonDestinationHistorian.Size = new System.Drawing.Size(90, 17);
            this.radioButtonDestinationHistorian.TabIndex = 20;
            this.radioButtonDestinationHistorian.TabStop = true;
            this.radioButtonDestinationHistorian.Tag = "openHistorian";
            this.radioButtonDestinationHistorian.Text = "openHistorian";
            this.radioButtonDestinationHistorian.UseVisualStyleBackColor = true;
            this.radioButtonDestinationHistorian.CheckedChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelHistorianName
            // 
            this.labelHistorianName.AutoSize = true;
            this.labelHistorianName.Location = new System.Drawing.Point(4, 54);
            this.labelHistorianName.Name = "labelHistorianName";
            this.labelHistorianName.Size = new System.Drawing.Size(79, 13);
            this.labelHistorianName.TabIndex = 18;
            this.labelHistorianName.Text = "Historian Name";
            // 
            // textBoxHistorianName
            // 
            this.textBoxHistorianName.Location = new System.Drawing.Point(89, 51);
            this.textBoxHistorianName.Name = "textBoxHistorianName";
            this.textBoxHistorianName.Size = new System.Drawing.Size(391, 20);
            this.textBoxHistorianName.TabIndex = 19;
            this.textBoxHistorianName.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelSourceHistorianMetaDataPort
            // 
            this.labelSourceHistorianMetaDataPort.AutoSize = true;
            this.labelSourceHistorianMetaDataPort.Location = new System.Drawing.Point(9, 79);
            this.labelSourceHistorianMetaDataPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSourceHistorianMetaDataPort.Name = "labelSourceHistorianMetaDataPort";
            this.labelSourceHistorianMetaDataPort.Size = new System.Drawing.Size(80, 13);
            this.labelSourceHistorianMetaDataPort.TabIndex = 4;
            this.labelSourceHistorianMetaDataPort.Text = "Meta-data Port:";
            this.labelSourceHistorianMetaDataPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSourceHistorianDataPort
            // 
            this.labelSourceHistorianDataPort.AutoSize = true;
            this.labelSourceHistorianDataPort.Location = new System.Drawing.Point(34, 51);
            this.labelSourceHistorianDataPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSourceHistorianDataPort.Name = "labelSourceHistorianDataPort";
            this.labelSourceHistorianDataPort.Size = new System.Drawing.Size(55, 13);
            this.labelSourceHistorianDataPort.TabIndex = 2;
            this.labelSourceHistorianDataPort.Text = "Data Port:";
            this.labelSourceHistorianDataPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskedTextBoxHistorianMetadataPort
            // 
            this.maskedTextBoxHistorianMetadataPort.Location = new System.Drawing.Point(93, 76);
            this.maskedTextBoxHistorianMetadataPort.Margin = new System.Windows.Forms.Padding(2);
            this.maskedTextBoxHistorianMetadataPort.Mask = "00000";
            this.maskedTextBoxHistorianMetadataPort.Name = "maskedTextBoxHistorianMetadataPort";
            this.maskedTextBoxHistorianMetadataPort.Size = new System.Drawing.Size(41, 20);
            this.maskedTextBoxHistorianMetadataPort.TabIndex = 5;
            this.maskedTextBoxHistorianMetadataPort.Text = "6175";
            this.maskedTextBoxHistorianMetadataPort.ValidatingType = typeof(int);
            this.maskedTextBoxHistorianMetadataPort.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // maskedTextBoxHistorianDataPort
            // 
            this.maskedTextBoxHistorianDataPort.Location = new System.Drawing.Point(93, 48);
            this.maskedTextBoxHistorianDataPort.Margin = new System.Windows.Forms.Padding(2);
            this.maskedTextBoxHistorianDataPort.Mask = "00000";
            this.maskedTextBoxHistorianDataPort.Name = "maskedTextBoxHistorianDataPort";
            this.maskedTextBoxHistorianDataPort.Size = new System.Drawing.Size(41, 20);
            this.maskedTextBoxHistorianDataPort.TabIndex = 3;
            this.maskedTextBoxHistorianDataPort.Text = "38402";
            this.maskedTextBoxHistorianDataPort.ValidatingType = typeof(int);
            this.maskedTextBoxHistorianDataPort.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // textBoxHistorianHostAddress
            // 
            this.textBoxHistorianHostAddress.Location = new System.Drawing.Point(93, 20);
            this.textBoxHistorianHostAddress.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxHistorianHostAddress.Name = "textBoxHistorianHostAddress";
            this.textBoxHistorianHostAddress.Size = new System.Drawing.Size(88, 20);
            this.textBoxHistorianHostAddress.TabIndex = 1;
            this.textBoxHistorianHostAddress.Text = "localhost";
            this.textBoxHistorianHostAddress.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelSourceHistorianHostAddress
            // 
            this.labelSourceHistorianHostAddress.AutoSize = true;
            this.labelSourceHistorianHostAddress.Location = new System.Drawing.Point(16, 23);
            this.labelSourceHistorianHostAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSourceHistorianHostAddress.Name = "labelSourceHistorianHostAddress";
            this.labelSourceHistorianHostAddress.Size = new System.Drawing.Size(73, 13);
            this.labelSourceHistorianHostAddress.TabIndex = 0;
            this.labelSourceHistorianHostAddress.Text = "Host Address:";
            this.labelSourceHistorianHostAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxHistorianInstanceName
            // 
            this.textBoxHistorianInstanceName.Location = new System.Drawing.Point(93, 104);
            this.textBoxHistorianInstanceName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxHistorianInstanceName.Name = "textBoxHistorianInstanceName";
            this.textBoxHistorianInstanceName.Size = new System.Drawing.Size(41, 20);
            this.textBoxHistorianInstanceName.TabIndex = 7;
            this.textBoxHistorianInstanceName.Text = "PPA";
            this.textBoxHistorianInstanceName.TextChanged += new System.EventHandler(this.FormElementChanged);
            // 
            // labelSourceHistorianInstanceName
            // 
            this.labelSourceHistorianInstanceName.AutoSize = true;
            this.labelSourceHistorianInstanceName.Location = new System.Drawing.Point(7, 107);
            this.labelSourceHistorianInstanceName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSourceHistorianInstanceName.Name = "labelSourceHistorianInstanceName";
            this.labelSourceHistorianInstanceName.Size = new System.Drawing.Size(82, 13);
            this.labelSourceHistorianInstanceName.TabIndex = 6;
            this.labelSourceHistorianInstanceName.Text = "Instance Name:";
            this.labelSourceHistorianInstanceName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBoxSourceHistorian
            // 
            this.groupBoxSourceHistorian.Controls.Add(this.labelSourceHistorianInstanceName);
            this.groupBoxSourceHistorian.Controls.Add(this.textBoxHistorianInstanceName);
            this.groupBoxSourceHistorian.Controls.Add(this.labelSourceHistorianHostAddress);
            this.groupBoxSourceHistorian.Controls.Add(this.textBoxHistorianHostAddress);
            this.groupBoxSourceHistorian.Controls.Add(this.maskedTextBoxHistorianDataPort);
            this.groupBoxSourceHistorian.Controls.Add(this.maskedTextBoxHistorianMetadataPort);
            this.groupBoxSourceHistorian.Controls.Add(this.labelSourceHistorianDataPort);
            this.groupBoxSourceHistorian.Controls.Add(this.labelSourceHistorianMetaDataPort);
            this.groupBoxSourceHistorian.Location = new System.Drawing.Point(8, 12);
            this.groupBoxSourceHistorian.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxSourceHistorian.Name = "groupBoxSourceHistorian";
            this.groupBoxSourceHistorian.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxSourceHistorian.Size = new System.Drawing.Size(194, 150);
            this.groupBoxSourceHistorian.TabIndex = 0;
            this.groupBoxSourceHistorian.TabStop = false;
            this.groupBoxSourceHistorian.Text = "&Source Historian";
            // 
            // groupBoxGeneralSettings
            // 
            this.groupBoxGeneralSettings.Controls.Add(this.maskedTextBoxMessageInterval);
            this.groupBoxGeneralSettings.Controls.Add(this.checkBoxEnableLogging);
            this.groupBoxGeneralSettings.Controls.Add(this.labelMessageInterval);
            this.groupBoxGeneralSettings.Location = new System.Drawing.Point(8, 167);
            this.groupBoxGeneralSettings.Name = "groupBoxGeneralSettings";
            this.groupBoxGeneralSettings.Size = new System.Drawing.Size(194, 101);
            this.groupBoxGeneralSettings.TabIndex = 25;
            this.groupBoxGeneralSettings.TabStop = false;
            this.groupBoxGeneralSettings.Text = "General Settings";
            // 
            // BenchmarkBerkeleyDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 712);
            this.Controls.Add(this.groupBoxGeneralSettings);
            this.Controls.Add(this.groupBoxDestination);
            this.Controls.Add(this.groupBoxSource);
            this.Controls.Add(this.groupBoxCsvSettings);
            this.Controls.Add(this.groupBoxHistorianSettings);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.groupBoxMessages);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBoxSourceHistorian);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(675, 600);
            this.Name = "BenchmarkBerkeleyDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Benchmark Berkeley DB Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BenchmarkBerkeleyDB_FormClosing);
            this.Load += new System.EventHandler(this.BenchmarkBerkeleyDB_Load);
            this.groupBoxMessages.ResumeLayout(false);
            this.groupBoxMessages.PerformLayout();
            this.groupBoxHistorianSettings.ResumeLayout(false);
            this.groupBoxHistorianSettings.PerformLayout();
            this.groupBoxCsvSettings.ResumeLayout(false);
            this.groupBoxCsvSettings.PerformLayout();
            this.groupBoxSource.ResumeLayout(false);
            this.groupBoxSource.PerformLayout();
            this.groupBoxDestination.ResumeLayout(false);
            this.groupBoxDestination.PerformLayout();
            this.groupBoxSourceHistorian.ResumeLayout(false);
            this.groupBoxSourceHistorian.PerformLayout();
            this.groupBoxGeneralSettings.ResumeLayout(false);
            this.groupBoxGeneralSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.GroupBox groupBoxMessages;
        private System.Windows.Forms.TextBox textBoxMessageOutput;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.GroupBox groupBoxHistorianSettings;
        private System.Windows.Forms.Label labelEndTime;
        private System.Windows.Forms.Label labelStartTime;
        private System.Windows.Forms.Label labelFrameRate;
        private System.Windows.Forms.Label labelSeconds;
        private System.Windows.Forms.Label labelPerSec;
        private System.Windows.Forms.Label labelMetaDataTimeout;
        private System.Windows.Forms.Label labelMessageInterval;
        private System.Windows.Forms.Label labelPointList;
        public System.Windows.Forms.DateTimePicker dateTimePickerEndTime;
        public System.Windows.Forms.DateTimePicker dateTimePickerSourceTime;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxFrameRate;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxMetadataTimeout;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxMessageInterval;
        public System.Windows.Forms.CheckBox checkBoxEnableLogging;
        public System.Windows.Forms.TextBox textBoxPointList;
        private System.Windows.Forms.Button buttonBrowseHistorianArchive;
        private System.Windows.Forms.Label labelDestinationDirectory;
        public System.Windows.Forms.TextBox textBoxHistorianArchive;
        public System.Windows.Forms.CheckBox checkBoxUseUTCTime;
        private System.Windows.Forms.GroupBox groupBoxCsvSettings;
        private System.Windows.Forms.Button buttonBrowseCsvSource;
        public System.Windows.Forms.TextBox textBoxCsvSource;
        private System.Windows.Forms.Label labelSourceFile;
        private System.Windows.Forms.Label labelSourceHistorianMetaDataPort;
        private System.Windows.Forms.Label labelSourceHistorianDataPort;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxHistorianMetadataPort;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxHistorianDataPort;
        public System.Windows.Forms.TextBox textBoxHistorianHostAddress;
        private System.Windows.Forms.Label labelSourceHistorianHostAddress;
        public System.Windows.Forms.TextBox textBoxHistorianInstanceName;
        private System.Windows.Forms.Label labelSourceHistorianInstanceName;
        private System.Windows.Forms.GroupBox groupBoxSourceHistorian;
        private System.Windows.Forms.Label labelHistorianName;
        public System.Windows.Forms.TextBox textBoxHistorianName;
        private System.Windows.Forms.GroupBox groupBoxGeneralSettings;
        public System.Windows.Forms.RadioButton radioButtonSourceHistorian;
        public System.Windows.Forms.RadioButton radioButtonSourceCsv;
        public System.Windows.Forms.RadioButton radioButtonDestinationBerkeley;
        public System.Windows.Forms.RadioButton radioButtonDestinationHistorian;
        public System.Windows.Forms.GroupBox groupBoxSource;
        public System.Windows.Forms.GroupBox groupBoxDestination;
    }
}