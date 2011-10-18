namespace cleanGatherer
{
    partial class Gatherer
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
            this.btnToggle = new System.Windows.Forms.Button();
            this.btnLoadProfile = new System.Windows.Forms.Button();
            this.tbLogBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDeaths = new System.Windows.Forms.Label();
            this.lblKills = new System.Windows.Forms.Label();
            this.lblHarvests = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cbMount = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnToggle
            // 
            this.btnToggle.Location = new System.Drawing.Point(12, 12);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Size = new System.Drawing.Size(75, 23);
            this.btnToggle.TabIndex = 0;
            this.btnToggle.Text = "Start";
            this.btnToggle.UseVisualStyleBackColor = true;
            this.btnToggle.Click += new System.EventHandler(this.btnToggle_Click);
            // 
            // btnLoadProfile
            // 
            this.btnLoadProfile.Location = new System.Drawing.Point(12, 41);
            this.btnLoadProfile.Name = "btnLoadProfile";
            this.btnLoadProfile.Size = new System.Drawing.Size(75, 23);
            this.btnLoadProfile.TabIndex = 1;
            this.btnLoadProfile.Text = "Load...";
            this.btnLoadProfile.UseVisualStyleBackColor = true;
            this.btnLoadProfile.Click += new System.EventHandler(this.btnLoadProfile_Click);
            // 
            // tbLogBox
            // 
            this.tbLogBox.BackColor = System.Drawing.Color.White;
            this.tbLogBox.Location = new System.Drawing.Point(12, 70);
            this.tbLogBox.Multiline = true;
            this.tbLogBox.Name = "tbLogBox";
            this.tbLogBox.ReadOnly = true;
            this.tbLogBox.Size = new System.Drawing.Size(350, 214);
            this.tbLogBox.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDeaths);
            this.groupBox1.Controls.Add(this.lblKills);
            this.groupBox1.Controls.Add(this.lblHarvests);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(208, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 52);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Statistics:";
            // 
            // lblDeaths
            // 
            this.lblDeaths.AutoSize = true;
            this.lblDeaths.Location = new System.Drawing.Point(98, 29);
            this.lblDeaths.Name = "lblDeaths";
            this.lblDeaths.Size = new System.Drawing.Size(13, 13);
            this.lblDeaths.TabIndex = 5;
            this.lblDeaths.Text = "0";
            // 
            // lblKills
            // 
            this.lblKills.AutoSize = true;
            this.lblKills.Location = new System.Drawing.Point(64, 29);
            this.lblKills.Name = "lblKills";
            this.lblKills.Size = new System.Drawing.Size(13, 13);
            this.lblKills.TabIndex = 7;
            this.lblKills.Text = "0";
            // 
            // lblHarvests
            // 
            this.lblHarvests.AutoSize = true;
            this.lblHarvests.Location = new System.Drawing.Point(6, 29);
            this.lblHarvests.Name = "lblHarvests";
            this.lblHarvests.Size = new System.Drawing.Size(13, 13);
            this.lblHarvests.TabIndex = 4;
            this.lblHarvests.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(98, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Deaths:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Kills:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Harvests:";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cbMount
            // 
            this.cbMount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMount.FormattingEnabled = true;
            this.cbMount.Location = new System.Drawing.Point(87, 12);
            this.cbMount.Name = "cbMount";
            this.cbMount.Size = new System.Drawing.Size(121, 21);
            this.cbMount.TabIndex = 4;
            this.cbMount.SelectedIndexChanged += new System.EventHandler(this.cbMount_SelectedIndexChanged);
            // 
            // Gatherer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 297);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbMount);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbLogBox);
            this.Controls.Add(this.btnLoadProfile);
            this.Controls.Add(this.btnToggle);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Gatherer";
            this.Text = "cleanGatherer";
            this.Load += new System.EventHandler(this.Gatherer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnToggle;
        private System.Windows.Forms.Button btnLoadProfile;
        private System.Windows.Forms.TextBox tbLogBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDeaths;
        private System.Windows.Forms.Label lblKills;
        private System.Windows.Forms.Label lblHarvests;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cbMount;
        private System.Windows.Forms.Button button1;
    }
}

