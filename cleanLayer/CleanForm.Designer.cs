namespace cleanLayer
{
    partial class CleanForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CleanForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblAuras = new System.Windows.Forms.Label();
            this.lblSpells = new System.Windows.Forms.Label();
            this.lblAttackers = new System.Windows.Forms.Label();
            this.lblEnemies = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBotSettings = new System.Windows.Forms.Button();
            this.cbBots = new System.Windows.Forms.ComboBox();
            this.btnBotStop = new System.Windows.Forms.Button();
            this.btnBotStart = new System.Windows.Forms.Button();
            this.cbBrains = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pbPower = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.pbHealth = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnScriptStop = new System.Windows.Forms.Button();
            this.btnScriptStart = new System.Windows.Forms.Button();
            this.lstScripts = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSpellDump = new System.Windows.Forms.Button();
            this.tbLua = new System.Windows.Forms.TextBox();
            this.btnExecuteLua = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnMountUp = new System.Windows.Forms.Button();
            this.btnPathTo = new System.Windows.Forms.Button();
            this.lstLocations = new System.Windows.Forms.ListBox();
            this.rbLogBox = new System.Windows.Forms.RichTextBox();
            this.GUITimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(384, 234);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(376, 208);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblAuras);
            this.groupBox3.Controls.Add(this.lblSpells);
            this.groupBox3.Controls.Add(this.lblAttackers);
            this.groupBox3.Controls.Add(this.lblEnemies);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(8, 69);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(185, 73);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Information";
            // 
            // lblAuras
            // 
            this.lblAuras.AutoSize = true;
            this.lblAuras.Location = new System.Drawing.Point(67, 55);
            this.lblAuras.Name = "lblAuras";
            this.lblAuras.Size = new System.Drawing.Size(63, 13);
            this.lblAuras.TabIndex = 7;
            this.lblAuras.Text = "<unknown>";
            // 
            // lblSpells
            // 
            this.lblSpells.AutoSize = true;
            this.lblSpells.Location = new System.Drawing.Point(67, 42);
            this.lblSpells.Name = "lblSpells";
            this.lblSpells.Size = new System.Drawing.Size(63, 13);
            this.lblSpells.TabIndex = 6;
            this.lblSpells.Text = "<unknown>";
            // 
            // lblAttackers
            // 
            this.lblAttackers.AutoSize = true;
            this.lblAttackers.Location = new System.Drawing.Point(67, 29);
            this.lblAttackers.Name = "lblAttackers";
            this.lblAttackers.Size = new System.Drawing.Size(63, 13);
            this.lblAttackers.TabIndex = 5;
            this.lblAttackers.Text = "<unknown>";
            // 
            // lblEnemies
            // 
            this.lblEnemies.AutoSize = true;
            this.lblEnemies.Location = new System.Drawing.Point(67, 16);
            this.lblEnemies.Name = "lblEnemies";
            this.lblEnemies.Size = new System.Drawing.Size(63, 13);
            this.lblEnemies.TabIndex = 4;
            this.lblEnemies.Text = "<unknown>";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Auras:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Spells:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Attackers:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enemies:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "WPF GUI";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBotSettings);
            this.groupBox1.Controls.Add(this.cbBots);
            this.groupBox1.Controls.Add(this.btnBotStop);
            this.groupBox1.Controls.Add(this.btnBotStart);
            this.groupBox1.Controls.Add(this.cbBrains);
            this.groupBox1.Location = new System.Drawing.Point(199, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(169, 136);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Combat";
            // 
            // btnBotSettings
            // 
            this.btnBotSettings.Enabled = false;
            this.btnBotSettings.Image = global::cleanLayer.Properties.Resources.cog;
            this.btnBotSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBotSettings.Location = new System.Drawing.Point(7, 46);
            this.btnBotSettings.Name = "btnBotSettings";
            this.btnBotSettings.Size = new System.Drawing.Size(156, 23);
            this.btnBotSettings.TabIndex = 7;
            this.btnBotSettings.Text = "Settings";
            this.btnBotSettings.UseVisualStyleBackColor = true;
            this.btnBotSettings.Click += new System.EventHandler(this.btnBotSettings_Click);
            // 
            // cbBots
            // 
            this.cbBots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBots.FormattingEnabled = true;
            this.cbBots.Location = new System.Drawing.Point(7, 19);
            this.cbBots.Name = "cbBots";
            this.cbBots.Size = new System.Drawing.Size(156, 21);
            this.cbBots.TabIndex = 6;
            this.cbBots.SelectedIndexChanged += new System.EventHandler(this.cbBots_SelectedIndexChanged);
            // 
            // btnBotStop
            // 
            this.btnBotStop.Enabled = false;
            this.btnBotStop.Image = global::cleanLayer.Properties.Resources.control_stop_blue;
            this.btnBotStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBotStop.Location = new System.Drawing.Point(88, 102);
            this.btnBotStop.Name = "btnBotStop";
            this.btnBotStop.Size = new System.Drawing.Size(75, 23);
            this.btnBotStop.TabIndex = 5;
            this.btnBotStop.Text = "Stop";
            this.btnBotStop.UseVisualStyleBackColor = true;
            this.btnBotStop.Click += new System.EventHandler(this.btnBotStop_Click);
            // 
            // btnBotStart
            // 
            this.btnBotStart.Image = global::cleanLayer.Properties.Resources.control_play_blue;
            this.btnBotStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBotStart.Location = new System.Drawing.Point(7, 102);
            this.btnBotStart.Name = "btnBotStart";
            this.btnBotStart.Size = new System.Drawing.Size(75, 23);
            this.btnBotStart.TabIndex = 5;
            this.btnBotStart.Text = "Start";
            this.btnBotStart.UseVisualStyleBackColor = true;
            this.btnBotStart.Click += new System.EventHandler(this.btnBotStart_Click);
            // 
            // cbBrains
            // 
            this.cbBrains.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBrains.FormattingEnabled = true;
            this.cbBrains.Location = new System.Drawing.Point(7, 75);
            this.cbBrains.Name = "cbBrains";
            this.cbBrains.Size = new System.Drawing.Size(156, 21);
            this.cbBrains.TabIndex = 3;
            this.cbBrains.SelectedIndexChanged += new System.EventHandler(this.cbBrains_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pbPower);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.pbHealth);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 57);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Player";
            // 
            // pbPower
            // 
            this.pbPower.Location = new System.Drawing.Point(53, 35);
            this.pbPower.Name = "pbPower";
            this.pbPower.Size = new System.Drawing.Size(100, 13);
            this.pbPower.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbPower.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Power:";
            // 
            // pbHealth
            // 
            this.pbHealth.Location = new System.Drawing.Point(53, 16);
            this.pbHealth.Name = "pbHealth";
            this.pbHealth.Size = new System.Drawing.Size(100, 13);
            this.pbHealth.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbHealth.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Health:";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Transparent;
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Controls.Add(this.btnScriptStop);
            this.tabPage3.Controls.Add(this.btnScriptStart);
            this.tabPage3.Controls.Add(this.lstScripts);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(376, 208);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Scripts";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(152, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(216, 190);
            this.panel1.TabIndex = 3;
            // 
            // btnScriptStop
            // 
            this.btnScriptStop.Image = global::cleanLayer.Properties.Resources.control_stop_blue;
            this.btnScriptStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnScriptStop.Location = new System.Drawing.Point(81, 173);
            this.btnScriptStop.Name = "btnScriptStop";
            this.btnScriptStop.Size = new System.Drawing.Size(65, 23);
            this.btnScriptStop.TabIndex = 2;
            this.btnScriptStop.Text = "Stop";
            this.btnScriptStop.UseVisualStyleBackColor = true;
            this.btnScriptStop.Click += new System.EventHandler(this.btnScriptStop_Click);
            // 
            // btnScriptStart
            // 
            this.btnScriptStart.Image = global::cleanLayer.Properties.Resources.control_play_blue;
            this.btnScriptStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnScriptStart.Location = new System.Drawing.Point(6, 173);
            this.btnScriptStart.Name = "btnScriptStart";
            this.btnScriptStart.Size = new System.Drawing.Size(65, 23);
            this.btnScriptStart.TabIndex = 1;
            this.btnScriptStart.Text = "Start";
            this.btnScriptStart.UseVisualStyleBackColor = true;
            this.btnScriptStart.Click += new System.EventHandler(this.btnScriptStart_Click);
            // 
            // lstScripts
            // 
            this.lstScripts.FormattingEnabled = true;
            this.lstScripts.Location = new System.Drawing.Point(6, 6);
            this.lstScripts.Name = "lstScripts";
            this.lstScripts.Size = new System.Drawing.Size(140, 160);
            this.lstScripts.TabIndex = 0;
            this.lstScripts.SelectedIndexChanged += new System.EventHandler(this.lstScripts_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnSpellDump);
            this.tabPage2.Controls.Add(this.tbLua);
            this.tabPage2.Controls.Add(this.btnExecuteLua);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(376, 208);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Debug";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnSpellDump
            // 
            this.btnSpellDump.Location = new System.Drawing.Point(8, 32);
            this.btnSpellDump.Name = "btnSpellDump";
            this.btnSpellDump.Size = new System.Drawing.Size(75, 23);
            this.btnSpellDump.TabIndex = 11;
            this.btnSpellDump.Text = "Spell dump";
            this.btnSpellDump.UseVisualStyleBackColor = true;
            this.btnSpellDump.Click += new System.EventHandler(this.btnSpellDump_Click);
            // 
            // tbLua
            // 
            this.tbLua.Location = new System.Drawing.Point(8, 6);
            this.tbLua.Name = "tbLua";
            this.tbLua.Size = new System.Drawing.Size(120, 20);
            this.tbLua.TabIndex = 10;
            // 
            // btnExecuteLua
            // 
            this.btnExecuteLua.Location = new System.Drawing.Point(134, 4);
            this.btnExecuteLua.Name = "btnExecuteLua";
            this.btnExecuteLua.Size = new System.Drawing.Size(75, 23);
            this.btnExecuteLua.TabIndex = 9;
            this.btnExecuteLua.Text = "Execute";
            this.btnExecuteLua.UseVisualStyleBackColor = true;
            this.btnExecuteLua.Click += new System.EventHandler(this.btnExecuteLua_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnMountUp);
            this.tabPage4.Controls.Add(this.btnPathTo);
            this.tabPage4.Controls.Add(this.lstLocations);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(376, 208);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Locations";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnMountUp
            // 
            this.btnMountUp.Location = new System.Drawing.Point(186, 169);
            this.btnMountUp.Name = "btnMountUp";
            this.btnMountUp.Size = new System.Drawing.Size(75, 23);
            this.btnMountUp.TabIndex = 2;
            this.btnMountUp.Text = "Mount";
            this.btnMountUp.UseVisualStyleBackColor = true;
            // 
            // btnPathTo
            // 
            this.btnPathTo.Location = new System.Drawing.Point(267, 169);
            this.btnPathTo.Name = "btnPathTo";
            this.btnPathTo.Size = new System.Drawing.Size(75, 23);
            this.btnPathTo.TabIndex = 1;
            this.btnPathTo.Text = "Path to";
            this.btnPathTo.UseVisualStyleBackColor = true;
            this.btnPathTo.Click += new System.EventHandler(this.btnPathTo_Click);
            // 
            // lstLocations
            // 
            this.lstLocations.FormattingEnabled = true;
            this.lstLocations.Location = new System.Drawing.Point(6, 6);
            this.lstLocations.Name = "lstLocations";
            this.lstLocations.Size = new System.Drawing.Size(336, 160);
            this.lstLocations.TabIndex = 0;
            // 
            // rbLogBox
            // 
            this.rbLogBox.BackColor = System.Drawing.Color.White;
            this.rbLogBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rbLogBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.rbLogBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rbLogBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rbLogBox.Location = new System.Drawing.Point(0, 234);
            this.rbLogBox.Name = "rbLogBox";
            this.rbLogBox.ReadOnly = true;
            this.rbLogBox.Size = new System.Drawing.Size(384, 278);
            this.rbLogBox.TabIndex = 0;
            this.rbLogBox.Text = "";
            // 
            // GUITimer
            // 
            this.GUITimer.Enabled = true;
            this.GUITimer.Interval = 500;
            this.GUITimer.Tick += new System.EventHandler(this.GUITimer_Tick);
            // 
            // CleanForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(384, 512);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.rbLogBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CleanForm";
            this.Text = "cleanLayer";
            this.Load += new System.EventHandler(this.CleanForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar pbPower;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pbHealth;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.RichTextBox rbLogBox;
        private System.Windows.Forms.ComboBox cbBrains;
        private System.Windows.Forms.TextBox tbLua;
        private System.Windows.Forms.Button btnExecuteLua;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnScriptStop;
        private System.Windows.Forms.Button btnScriptStart;
        private System.Windows.Forms.ListBox lstScripts;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBotStop;
        private System.Windows.Forms.Button btnBotStart;
        private System.Windows.Forms.Timer GUITimer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSpellDump;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnMountUp;
        private System.Windows.Forms.Button btnPathTo;
        private System.Windows.Forms.ListBox lstLocations;
        private System.Windows.Forms.ComboBox cbBots;
        private System.Windows.Forms.Button btnBotSettings;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblAuras;
        private System.Windows.Forms.Label lblSpells;
        private System.Windows.Forms.Label lblAttackers;
        private System.Windows.Forms.Label lblEnemies;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
    }
}