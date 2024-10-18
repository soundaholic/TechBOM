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
            Button_Start = new Button();
            Button_Cancel = new Button();
            comboBoxProjects = new ComboBox();
            lbl_Customer = new Label();
            textBox_Customer = new TextBox();
            lbl_OEM = new Label();
            textBox_Oem = new TextBox();
            lbl_Date = new Label();
            lbl_Project = new Label();
            textBox_Date = new TextBox();
            textBox_Project = new TextBox();
            lbl_DrawingNumber = new Label();
            lbl_Plant = new Label();
            textBox_DrawingNumber = new TextBox();
            lbl_Line = new Label();
            textBox_Plant = new TextBox();
            textBox_Line = new TextBox();
            label3 = new Label();
            label4 = new Label();
            label8 = new Label();
            textBox_Version = new TextBox();
            textBox_Station = new TextBox();
            textBox_Name = new TextBox();
            groupBox_HeadData = new GroupBox();
            lbl_ScanDepth = new Label();
            comboBox_ScanDepth = new ComboBox();
            lbl_Language = new Label();
            comboBox_Language = new ComboBox();
            lbl_Status = new Label();
            groupBox_HeadData.SuspendLayout();
            SuspendLayout();
            // 
            // Button_Start
            // 
            Button_Start.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            Button_Start.Location = new Point(544, 314);
            Button_Start.Margin = new Padding(4, 3, 4, 3);
            Button_Start.Name = "Button_Start";
            Button_Start.Size = new Size(104, 30);
            Button_Start.TabIndex = 13;
            Button_Start.Text = "Start";
            Button_Start.UseVisualStyleBackColor = true;
            Button_Start.Click += Button_Start_Click;
            // 
            // Button_Cancel
            // 
            Button_Cancel.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            Button_Cancel.Location = new Point(667, 314);
            Button_Cancel.Margin = new Padding(4, 3, 4, 3);
            Button_Cancel.Name = "Button_Cancel";
            Button_Cancel.Size = new Size(104, 30);
            Button_Cancel.TabIndex = 14;
            Button_Cancel.Text = "Beenden";
            Button_Cancel.UseVisualStyleBackColor = true;
            Button_Cancel.Click += Button_Cancel_Click;
            // 
            // comboBoxProjects
            // 
            comboBoxProjects.FormattingEnabled = true;
            comboBoxProjects.Location = new Point(43, 580);
            comboBoxProjects.Margin = new Padding(4, 3, 4, 3);
            comboBoxProjects.Name = "comboBoxProjects";
            comboBoxProjects.Size = new Size(228, 23);
            comboBoxProjects.TabIndex = 2;
            comboBoxProjects.Visible = false;
            // 
            // lbl_Customer
            // 
            lbl_Customer.AutoSize = true;
            lbl_Customer.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lbl_Customer.Location = new Point(7, 120);
            lbl_Customer.Margin = new Padding(4, 0, 4, 0);
            lbl_Customer.Name = "lbl_Customer";
            lbl_Customer.Size = new Size(71, 17);
            lbl_Customer.TabIndex = 0;
            lbl_Customer.Text = "Customer:";
            // 
            // textBox_Customer
            // 
            textBox_Customer.BackColor = SystemColors.MenuBar;
            textBox_Customer.Location = new Point(119, 115);
            textBox_Customer.Margin = new Padding(4, 3, 4, 3);
            textBox_Customer.Name = "textBox_Customer";
            textBox_Customer.Size = new Size(227, 25);
            textBox_Customer.TabIndex = 1;
            // 
            // lbl_OEM
            // 
            lbl_OEM.AutoSize = true;
            lbl_OEM.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lbl_OEM.Location = new Point(7, 155);
            lbl_OEM.Margin = new Padding(4, 0, 4, 0);
            lbl_OEM.Name = "lbl_OEM";
            lbl_OEM.Size = new Size(40, 17);
            lbl_OEM.TabIndex = 0;
            lbl_OEM.Text = "OEM:";
            // 
            // textBox_Oem
            // 
            textBox_Oem.BackColor = SystemColors.MenuBar;
            textBox_Oem.Location = new Point(119, 150);
            textBox_Oem.Margin = new Padding(4, 3, 4, 3);
            textBox_Oem.Name = "textBox_Oem";
            textBox_Oem.Size = new Size(227, 25);
            textBox_Oem.TabIndex = 2;
            // 
            // lbl_Date
            // 
            lbl_Date.AutoSize = true;
            lbl_Date.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lbl_Date.Location = new Point(7, 224);
            lbl_Date.Margin = new Padding(4, 0, 4, 0);
            lbl_Date.Name = "lbl_Date";
            lbl_Date.Size = new Size(36, 17);
            lbl_Date.TabIndex = 0;
            lbl_Date.Text = "Date";
            // 
            // lbl_Project
            // 
            lbl_Project.AutoSize = true;
            lbl_Project.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lbl_Project.Location = new Point(7, 190);
            lbl_Project.Margin = new Padding(4, 0, 4, 0);
            lbl_Project.Name = "lbl_Project";
            lbl_Project.Size = new Size(53, 17);
            lbl_Project.TabIndex = 0;
            lbl_Project.Text = "Project:";
            // 
            // textBox_Date
            // 
            textBox_Date.BackColor = SystemColors.MenuBar;
            textBox_Date.Location = new Point(119, 220);
            textBox_Date.Margin = new Padding(4, 3, 4, 3);
            textBox_Date.Name = "textBox_Date";
            textBox_Date.Size = new Size(227, 25);
            textBox_Date.TabIndex = 4;
            // 
            // textBox_Project
            // 
            textBox_Project.BackColor = SystemColors.MenuBar;
            textBox_Project.Location = new Point(119, 185);
            textBox_Project.Margin = new Padding(4, 3, 4, 3);
            textBox_Project.Name = "textBox_Project";
            textBox_Project.Size = new Size(227, 25);
            textBox_Project.TabIndex = 3;
            // 
            // lbl_DrawingNumber
            // 
            lbl_DrawingNumber.AutoSize = true;
            lbl_DrawingNumber.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lbl_DrawingNumber.Location = new Point(363, 194);
            lbl_DrawingNumber.Margin = new Padding(4, 0, 4, 0);
            lbl_DrawingNumber.Name = "lbl_DrawingNumber";
            lbl_DrawingNumber.Size = new Size(110, 17);
            lbl_DrawingNumber.TabIndex = 0;
            lbl_DrawingNumber.Text = "Drawingnumber:";
            // 
            // lbl_Plant
            // 
            lbl_Plant.AutoSize = true;
            lbl_Plant.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lbl_Plant.Location = new Point(363, 50);
            lbl_Plant.Margin = new Padding(4, 0, 4, 0);
            lbl_Plant.Name = "lbl_Plant";
            lbl_Plant.Size = new Size(42, 17);
            lbl_Plant.TabIndex = 0;
            lbl_Plant.Text = "Plant:";
            // 
            // textBox_DrawingNumber
            // 
            textBox_DrawingNumber.BackColor = SystemColors.MenuBar;
            textBox_DrawingNumber.Location = new Point(512, 190);
            textBox_DrawingNumber.Margin = new Padding(4, 3, 4, 3);
            textBox_DrawingNumber.Name = "textBox_DrawingNumber";
            textBox_DrawingNumber.Size = new Size(227, 25);
            textBox_DrawingNumber.TabIndex = 9;
            // 
            // lbl_Line
            // 
            lbl_Line.AutoSize = true;
            lbl_Line.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lbl_Line.Location = new Point(363, 87);
            lbl_Line.Margin = new Padding(4, 0, 4, 0);
            lbl_Line.Name = "lbl_Line";
            lbl_Line.Size = new Size(35, 17);
            lbl_Line.TabIndex = 0;
            lbl_Line.Text = "Line:";
            // 
            // textBox_Plant
            // 
            textBox_Plant.BackColor = SystemColors.MenuBar;
            textBox_Plant.Location = new Point(512, 46);
            textBox_Plant.Margin = new Padding(4, 3, 4, 3);
            textBox_Plant.Name = "textBox_Plant";
            textBox_Plant.Size = new Size(227, 25);
            textBox_Plant.TabIndex = 5;
            // 
            // textBox_Line
            // 
            textBox_Line.BackColor = SystemColors.MenuBar;
            textBox_Line.Location = new Point(512, 82);
            textBox_Line.Margin = new Padding(4, 3, 4, 3);
            textBox_Line.Name = "textBox_Line";
            textBox_Line.Size = new Size(227, 25);
            textBox_Line.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            label3.Location = new Point(363, 230);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(55, 17);
            label3.TabIndex = 0;
            label3.Text = "Version:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            label4.Location = new Point(363, 158);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(47, 17);
            label4.TabIndex = 0;
            label4.Text = "Name:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            label8.Location = new Point(363, 121);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(54, 17);
            label8.TabIndex = 0;
            label8.Text = "Station:";
            // 
            // textBox_Version
            // 
            textBox_Version.BackColor = SystemColors.MenuBar;
            textBox_Version.Location = new Point(512, 225);
            textBox_Version.Margin = new Padding(4, 3, 4, 3);
            textBox_Version.Name = "textBox_Version";
            textBox_Version.Size = new Size(227, 25);
            textBox_Version.TabIndex = 10;
            // 
            // textBox_Station
            // 
            textBox_Station.BackColor = SystemColors.MenuBar;
            textBox_Station.Location = new Point(512, 119);
            textBox_Station.Margin = new Padding(4, 3, 4, 3);
            textBox_Station.Name = "textBox_Station";
            textBox_Station.Size = new Size(227, 25);
            textBox_Station.TabIndex = 7;
            // 
            // textBox_Name
            // 
            textBox_Name.BackColor = SystemColors.MenuBar;
            textBox_Name.Location = new Point(512, 153);
            textBox_Name.Margin = new Padding(4, 3, 4, 3);
            textBox_Name.Name = "textBox_Name";
            textBox_Name.Size = new Size(227, 25);
            textBox_Name.TabIndex = 8;
            // 
            // groupBox_HeadData
            // 
            groupBox_HeadData.Controls.Add(lbl_ScanDepth);
            groupBox_HeadData.Controls.Add(comboBox_ScanDepth);
            groupBox_HeadData.Controls.Add(lbl_Language);
            groupBox_HeadData.Controls.Add(textBox_Name);
            groupBox_HeadData.Controls.Add(comboBox_Language);
            groupBox_HeadData.Controls.Add(textBox_Station);
            groupBox_HeadData.Controls.Add(textBox_Version);
            groupBox_HeadData.Controls.Add(label8);
            groupBox_HeadData.Controls.Add(label4);
            groupBox_HeadData.Controls.Add(label3);
            groupBox_HeadData.Controls.Add(textBox_Line);
            groupBox_HeadData.Controls.Add(textBox_Plant);
            groupBox_HeadData.Controls.Add(lbl_Line);
            groupBox_HeadData.Controls.Add(textBox_DrawingNumber);
            groupBox_HeadData.Controls.Add(lbl_Plant);
            groupBox_HeadData.Controls.Add(lbl_DrawingNumber);
            groupBox_HeadData.Controls.Add(textBox_Project);
            groupBox_HeadData.Controls.Add(textBox_Date);
            groupBox_HeadData.Controls.Add(lbl_Project);
            groupBox_HeadData.Controls.Add(lbl_Date);
            groupBox_HeadData.Controls.Add(textBox_Oem);
            groupBox_HeadData.Controls.Add(lbl_OEM);
            groupBox_HeadData.Controls.Add(textBox_Customer);
            groupBox_HeadData.Controls.Add(lbl_Customer);
            groupBox_HeadData.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            groupBox_HeadData.Location = new Point(17, 34);
            groupBox_HeadData.Margin = new Padding(4, 3, 4, 3);
            groupBox_HeadData.Name = "groupBox_HeadData";
            groupBox_HeadData.Padding = new Padding(4, 3, 4, 3);
            groupBox_HeadData.Size = new Size(755, 271);
            groupBox_HeadData.TabIndex = 0;
            groupBox_HeadData.TabStop = false;
            groupBox_HeadData.Text = "Kopfdaten";
            groupBox_HeadData.Enter += groupBox_HeadData_Enter;
            // 
            // lbl_ScanDepth
            // 
            lbl_ScanDepth.AutoSize = true;
            lbl_ScanDepth.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lbl_ScanDepth.Location = new Point(7, 50);
            lbl_ScanDepth.Margin = new Padding(4, 0, 4, 0);
            lbl_ScanDepth.Name = "lbl_ScanDepth";
            lbl_ScanDepth.Size = new Size(79, 17);
            lbl_ScanDepth.TabIndex = 17;
            lbl_ScanDepth.Text = "Scan depth:";
            // 
            // comboBox_ScanDepth
            // 
            comboBox_ScanDepth.BackColor = SystemColors.MenuBar;
            comboBox_ScanDepth.FormattingEnabled = true;
            comboBox_ScanDepth.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "All" });
            comboBox_ScanDepth.Location = new Point(119, 46);
            comboBox_ScanDepth.Margin = new Padding(4, 3, 4, 3);
            comboBox_ScanDepth.Name = "comboBox_ScanDepth";
            comboBox_ScanDepth.Size = new Size(87, 25);
            comboBox_ScanDepth.TabIndex = 16;
            // 
            // lbl_Language
            // 
            lbl_Language.AutoSize = true;
            lbl_Language.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lbl_Language.Location = new Point(7, 85);
            lbl_Language.Margin = new Padding(4, 0, 4, 0);
            lbl_Language.Name = "lbl_Language";
            lbl_Language.Size = new Size(67, 17);
            lbl_Language.TabIndex = 14;
            lbl_Language.Text = "Language";
            // 
            // comboBox_Language
            // 
            comboBox_Language.BackColor = SystemColors.MenuBar;
            comboBox_Language.FormattingEnabled = true;
            comboBox_Language.Items.AddRange(new object[] { "English", "German" });
            comboBox_Language.Location = new Point(119, 80);
            comboBox_Language.Margin = new Padding(4, 3, 4, 3);
            comboBox_Language.Name = "comboBox_Language";
            comboBox_Language.Size = new Size(140, 25);
            comboBox_Language.TabIndex = 13;
            // 
            // lbl_Status
            // 
            lbl_Status.AutoSize = true;
            lbl_Status.Font = new Font("Microsoft Sans Serif", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_Status.Location = new Point(31, 317);
            lbl_Status.Margin = new Padding(4, 0, 4, 0);
            lbl_Status.Name = "lbl_Status";
            lbl_Status.Size = new Size(0, 25);
            lbl_Status.TabIndex = 15;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 255);
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(791, 355);
            Controls.Add(lbl_Status);
            Controls.Add(comboBoxProjects);
            Controls.Add(Button_Cancel);
            Controls.Add(Button_Start);
            Controls.Add(groupBox_HeadData);
            ImeMode = ImeMode.Disable;
            Margin = new Padding(4, 3, 4, 3);
            Name = "MainForm";
            Padding = new Padding(22, 70, 22, 23);
            Resizable = false;
            RightToLeft = RightToLeft.No;
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            groupBox_HeadData.ResumeLayout(false);
            groupBox_HeadData.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Label lbl_Status;
        private System.Windows.Forms.Label lbl_Language;
        private System.Windows.Forms.ComboBox comboBox_Language;
        private System.Windows.Forms.Label lbl_ScanDepth;
        private System.Windows.Forms.ComboBox comboBox_ScanDepth;
    }
}

