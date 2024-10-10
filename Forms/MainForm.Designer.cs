namespace TechBOM
{
    partial class MainForm
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
            this.Button_Start = new System.Windows.Forms.Button();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.comboBoxProjects = new System.Windows.Forms.ComboBox();
            this.lbl_Customer = new System.Windows.Forms.Label();
            this.textBox_Customer = new System.Windows.Forms.TextBox();
            this.lbl_OEM = new System.Windows.Forms.Label();
            this.textBox_Oem = new System.Windows.Forms.TextBox();
            this.lbl_Date = new System.Windows.Forms.Label();
            this.lbl_Project = new System.Windows.Forms.Label();
            this.textBox_Date = new System.Windows.Forms.TextBox();
            this.textBox_Project = new System.Windows.Forms.TextBox();
            this.lbl_DrawingNumber = new System.Windows.Forms.Label();
            this.lbl_Plant = new System.Windows.Forms.Label();
            this.textBox_DrawingNumber = new System.Windows.Forms.TextBox();
            this.lbl_Line = new System.Windows.Forms.Label();
            this.textBox_Plant = new System.Windows.Forms.TextBox();
            this.textBox_Line = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_Version = new System.Windows.Forms.TextBox();
            this.textBox_Station = new System.Windows.Forms.TextBox();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.groupBox_HeadData = new System.Windows.Forms.GroupBox();
            this.lbl_Language = new System.Windows.Forms.Label();
            this.radioButton_Bg = new System.Windows.Forms.RadioButton();
            this.comboBox_Language = new System.Windows.Forms.ComboBox();
            this.radioButton_Zb = new System.Windows.Forms.RadioButton();
            this.lbl_Status = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_ScanDepth = new System.Windows.Forms.ComboBox();
            this.groupBox_HeadData.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Start
            // 
            this.Button_Start.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Button_Start.Location = new System.Drawing.Point(442, 264);
            this.Button_Start.Name = "Button_Start";
            this.Button_Start.Size = new System.Drawing.Size(88, 30);
            this.Button_Start.TabIndex = 13;
            this.Button_Start.Text = "Start";
            this.Button_Start.UseVisualStyleBackColor = true;
            this.Button_Start.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.Button_Cancel.Location = new System.Drawing.Point(549, 264);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(88, 30);
            this.Button_Cancel.TabIndex = 14;
            this.Button_Cancel.Text = "Beenden";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            this.Button_Cancel.Click += new System.EventHandler(this.Button_Cancel_Click);
            // 
            // comboBoxProjects
            // 
            this.comboBoxProjects.FormattingEnabled = true;
            this.comboBoxProjects.Location = new System.Drawing.Point(37, 503);
            this.comboBoxProjects.Name = "comboBoxProjects";
            this.comboBoxProjects.Size = new System.Drawing.Size(196, 21);
            this.comboBoxProjects.TabIndex = 2;
            this.comboBoxProjects.Visible = false;
            // 
            // lbl_Customer
            // 
            this.lbl_Customer.AutoSize = true;
            this.lbl_Customer.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_Customer.Location = new System.Drawing.Point(6, 104);
            this.lbl_Customer.Name = "lbl_Customer";
            this.lbl_Customer.Size = new System.Drawing.Size(71, 17);
            this.lbl_Customer.TabIndex = 0;
            this.lbl_Customer.Text = "Customer:";
            // 
            // textBox_Customer
            // 
            this.textBox_Customer.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox_Customer.Location = new System.Drawing.Point(102, 100);
            this.textBox_Customer.Name = "textBox_Customer";
            this.textBox_Customer.Size = new System.Drawing.Size(195, 25);
            this.textBox_Customer.TabIndex = 1;
            // 
            // lbl_OEM
            // 
            this.lbl_OEM.AutoSize = true;
            this.lbl_OEM.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_OEM.Location = new System.Drawing.Point(6, 134);
            this.lbl_OEM.Name = "lbl_OEM";
            this.lbl_OEM.Size = new System.Drawing.Size(40, 17);
            this.lbl_OEM.TabIndex = 0;
            this.lbl_OEM.Text = "OEM:";
            // 
            // textBox_Oem
            // 
            this.textBox_Oem.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox_Oem.Location = new System.Drawing.Point(102, 130);
            this.textBox_Oem.Name = "textBox_Oem";
            this.textBox_Oem.Size = new System.Drawing.Size(195, 25);
            this.textBox_Oem.TabIndex = 2;
            // 
            // lbl_Date
            // 
            this.lbl_Date.AutoSize = true;
            this.lbl_Date.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_Date.Location = new System.Drawing.Point(6, 194);
            this.lbl_Date.Name = "lbl_Date";
            this.lbl_Date.Size = new System.Drawing.Size(36, 17);
            this.lbl_Date.TabIndex = 0;
            this.lbl_Date.Text = "Date";
            // 
            // lbl_Project
            // 
            this.lbl_Project.AutoSize = true;
            this.lbl_Project.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_Project.Location = new System.Drawing.Point(6, 164);
            this.lbl_Project.Name = "lbl_Project";
            this.lbl_Project.Size = new System.Drawing.Size(53, 17);
            this.lbl_Project.TabIndex = 0;
            this.lbl_Project.Text = "Project:";
            // 
            // textBox_Date
            // 
            this.textBox_Date.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox_Date.Location = new System.Drawing.Point(102, 190);
            this.textBox_Date.Name = "textBox_Date";
            this.textBox_Date.Size = new System.Drawing.Size(195, 25);
            this.textBox_Date.TabIndex = 4;
            // 
            // textBox_Project
            // 
            this.textBox_Project.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox_Project.Location = new System.Drawing.Point(102, 160);
            this.textBox_Project.Name = "textBox_Project";
            this.textBox_Project.Size = new System.Drawing.Size(195, 25);
            this.textBox_Project.TabIndex = 3;
            // 
            // lbl_DrawingNumber
            // 
            this.lbl_DrawingNumber.AutoSize = true;
            this.lbl_DrawingNumber.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_DrawingNumber.Location = new System.Drawing.Point(311, 168);
            this.lbl_DrawingNumber.Name = "lbl_DrawingNumber";
            this.lbl_DrawingNumber.Size = new System.Drawing.Size(110, 17);
            this.lbl_DrawingNumber.TabIndex = 0;
            this.lbl_DrawingNumber.Text = "Drawingnumber:";
            // 
            // lbl_Plant
            // 
            this.lbl_Plant.AutoSize = true;
            this.lbl_Plant.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_Plant.Location = new System.Drawing.Point(311, 44);
            this.lbl_Plant.Name = "lbl_Plant";
            this.lbl_Plant.Size = new System.Drawing.Size(42, 17);
            this.lbl_Plant.TabIndex = 0;
            this.lbl_Plant.Text = "Plant:";
            // 
            // textBox_DrawingNumber
            // 
            this.textBox_DrawingNumber.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox_DrawingNumber.Location = new System.Drawing.Point(439, 164);
            this.textBox_DrawingNumber.Name = "textBox_DrawingNumber";
            this.textBox_DrawingNumber.Size = new System.Drawing.Size(195, 25);
            this.textBox_DrawingNumber.TabIndex = 9;
            // 
            // lbl_Line
            // 
            this.lbl_Line.AutoSize = true;
            this.lbl_Line.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_Line.Location = new System.Drawing.Point(311, 75);
            this.lbl_Line.Name = "lbl_Line";
            this.lbl_Line.Size = new System.Drawing.Size(35, 17);
            this.lbl_Line.TabIndex = 0;
            this.lbl_Line.Text = "Line:";
            // 
            // textBox_Plant
            // 
            this.textBox_Plant.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox_Plant.Location = new System.Drawing.Point(439, 40);
            this.textBox_Plant.Name = "textBox_Plant";
            this.textBox_Plant.Size = new System.Drawing.Size(195, 25);
            this.textBox_Plant.TabIndex = 5;
            // 
            // textBox_Line
            // 
            this.textBox_Line.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox_Line.Location = new System.Drawing.Point(439, 71);
            this.textBox_Line.Name = "textBox_Line";
            this.textBox_Line.Size = new System.Drawing.Size(195, 25);
            this.textBox_Line.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(311, 199);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Version:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(311, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Name:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(311, 106);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "Station:";
            // 
            // textBox_Version
            // 
            this.textBox_Version.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox_Version.Location = new System.Drawing.Point(439, 195);
            this.textBox_Version.Name = "textBox_Version";
            this.textBox_Version.Size = new System.Drawing.Size(195, 25);
            this.textBox_Version.TabIndex = 10;
            // 
            // textBox_Station
            // 
            this.textBox_Station.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox_Station.Location = new System.Drawing.Point(439, 102);
            this.textBox_Station.Name = "textBox_Station";
            this.textBox_Station.Size = new System.Drawing.Size(195, 25);
            this.textBox_Station.TabIndex = 7;
            // 
            // textBox_Name
            // 
            this.textBox_Name.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox_Name.Location = new System.Drawing.Point(439, 133);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(195, 25);
            this.textBox_Name.TabIndex = 8;
            // 
            // groupBox_HeadData
            // 
            this.groupBox_HeadData.Controls.Add(this.label1);
            this.groupBox_HeadData.Controls.Add(this.comboBox_ScanDepth);
            this.groupBox_HeadData.Controls.Add(this.lbl_Language);
            this.groupBox_HeadData.Controls.Add(this.textBox_Name);
            this.groupBox_HeadData.Controls.Add(this.comboBox_Language);
            this.groupBox_HeadData.Controls.Add(this.textBox_Station);
            this.groupBox_HeadData.Controls.Add(this.textBox_Version);
            this.groupBox_HeadData.Controls.Add(this.label8);
            this.groupBox_HeadData.Controls.Add(this.label4);
            this.groupBox_HeadData.Controls.Add(this.label3);
            this.groupBox_HeadData.Controls.Add(this.textBox_Line);
            this.groupBox_HeadData.Controls.Add(this.textBox_Plant);
            this.groupBox_HeadData.Controls.Add(this.lbl_Line);
            this.groupBox_HeadData.Controls.Add(this.textBox_DrawingNumber);
            this.groupBox_HeadData.Controls.Add(this.lbl_Plant);
            this.groupBox_HeadData.Controls.Add(this.lbl_DrawingNumber);
            this.groupBox_HeadData.Controls.Add(this.textBox_Project);
            this.groupBox_HeadData.Controls.Add(this.textBox_Date);
            this.groupBox_HeadData.Controls.Add(this.lbl_Project);
            this.groupBox_HeadData.Controls.Add(this.lbl_Date);
            this.groupBox_HeadData.Controls.Add(this.textBox_Oem);
            this.groupBox_HeadData.Controls.Add(this.lbl_OEM);
            this.groupBox_HeadData.Controls.Add(this.textBox_Customer);
            this.groupBox_HeadData.Controls.Add(this.lbl_Customer);
            this.groupBox_HeadData.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox_HeadData.Location = new System.Drawing.Point(12, 12);
            this.groupBox_HeadData.Name = "groupBox_HeadData";
            this.groupBox_HeadData.Size = new System.Drawing.Size(647, 236);
            this.groupBox_HeadData.TabIndex = 0;
            this.groupBox_HeadData.TabStop = false;
            this.groupBox_HeadData.Text = "Kopfdaten";
            this.groupBox_HeadData.Enter += new System.EventHandler(this.groupBox_HeadData_Enter);
            // 
            // lbl_Language
            // 
            this.lbl_Language.AutoSize = true;
            this.lbl_Language.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_Language.Location = new System.Drawing.Point(6, 74);
            this.lbl_Language.Name = "lbl_Language";
            this.lbl_Language.Size = new System.Drawing.Size(67, 17);
            this.lbl_Language.TabIndex = 14;
            this.lbl_Language.Text = "Language";
            // 
            // radioButton_Bg
            // 
            this.radioButton_Bg.AutoSize = true;
            this.radioButton_Bg.Location = new System.Drawing.Point(111, 269);
            this.radioButton_Bg.Name = "radioButton_Bg";
            this.radioButton_Bg.Size = new System.Drawing.Size(40, 17);
            this.radioButton_Bg.TabIndex = 12;
            this.radioButton_Bg.Text = "BG";
            this.radioButton_Bg.UseVisualStyleBackColor = true;
            this.radioButton_Bg.Visible = false;
            // 
            // comboBox_Language
            // 
            this.comboBox_Language.BackColor = System.Drawing.SystemColors.MenuBar;
            this.comboBox_Language.FormattingEnabled = true;
            this.comboBox_Language.Items.AddRange(new object[] {
            "English",
            "German"});
            this.comboBox_Language.Location = new System.Drawing.Point(102, 70);
            this.comboBox_Language.Name = "comboBox_Language";
            this.comboBox_Language.Size = new System.Drawing.Size(121, 25);
            this.comboBox_Language.TabIndex = 13;
            // 
            // radioButton_Zb
            // 
            this.radioButton_Zb.AutoSize = true;
            this.radioButton_Zb.Location = new System.Drawing.Point(67, 269);
            this.radioButton_Zb.Name = "radioButton_Zb";
            this.radioButton_Zb.Size = new System.Drawing.Size(39, 17);
            this.radioButton_Zb.TabIndex = 11;
            this.radioButton_Zb.Text = "ZB";
            this.radioButton_Zb.UseVisualStyleBackColor = true;
            this.radioButton_Zb.Visible = false;
            // 
            // lbl_Status
            // 
            this.lbl_Status.AutoSize = true;
            this.lbl_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Status.Location = new System.Drawing.Point(23, 267);
            this.lbl_Status.Name = "lbl_Status";
            this.lbl_Status.Size = new System.Drawing.Size(0, 25);
            this.lbl_Status.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "Scan depth:";
            // 
            // comboBox_ScanDepth
            // 
            this.comboBox_ScanDepth.BackColor = System.Drawing.SystemColors.MenuBar;
            this.comboBox_ScanDepth.FormattingEnabled = true;
            this.comboBox_ScanDepth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "All"});
            this.comboBox_ScanDepth.Location = new System.Drawing.Point(102, 40);
            this.comboBox_ScanDepth.Name = "comboBox_ScanDepth";
            this.comboBox_ScanDepth.Size = new System.Drawing.Size(75, 25);
            this.comboBox_ScanDepth.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(157)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(671, 311);
            this.Controls.Add(this.lbl_Status);
            this.Controls.Add(this.comboBoxProjects);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.radioButton_Bg);
            this.Controls.Add(this.Button_Start);
            this.Controls.Add(this.groupBox_HeadData);
            this.Controls.Add(this.radioButton_Zb);
            this.Name = "Form1";
            this.Text = "FFT TechBom - v1.5";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox_HeadData.ResumeLayout(false);
            this.groupBox_HeadData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Button_Start;
        private System.Windows.Forms.Button Button_Cancel;
        private System.Windows.Forms.ComboBox comboBoxProjects;
        private System.Windows.Forms.Label lbl_Customer;
        private System.Windows.Forms.TextBox textBox_Customer;
        private System.Windows.Forms.Label lbl_OEM;
        private System.Windows.Forms.TextBox textBox_Oem;
        private System.Windows.Forms.Label lbl_Date;
        private System.Windows.Forms.Label lbl_Project;
        private System.Windows.Forms.TextBox textBox_Date;
        private System.Windows.Forms.TextBox textBox_Project;
        private System.Windows.Forms.Label lbl_DrawingNumber;
        private System.Windows.Forms.Label lbl_Plant;
        private System.Windows.Forms.TextBox textBox_DrawingNumber;
        private System.Windows.Forms.Label lbl_Line;
        private System.Windows.Forms.TextBox textBox_Plant;
        private System.Windows.Forms.TextBox textBox_Line;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_Version;
        private System.Windows.Forms.TextBox textBox_Station;
        private System.Windows.Forms.TextBox textBox_Name;
        private System.Windows.Forms.GroupBox groupBox_HeadData;
        private System.Windows.Forms.RadioButton radioButton_Bg;
        private System.Windows.Forms.RadioButton radioButton_Zb;
        private System.Windows.Forms.Label lbl_Status;
        private System.Windows.Forms.Label lbl_Language;
        private System.Windows.Forms.ComboBox comboBox_Language;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_ScanDepth;
    }
}

