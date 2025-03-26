namespace TechBOM.Forms
{
    partial class QuelleSelection
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
            comboBox_Quelle = new ComboBox();
            button_OK = new Button();
            label1 = new Label();
            label_Name = new Label();
            label2 = new Label();
            button_Cancel = new Button();
            label3 = new Label();
            SuspendLayout();
            // 
            // comboBox_Quelle
            // 
            comboBox_Quelle.FormattingEnabled = true;
            comboBox_Quelle.Location = new Point(25, 107);
            comboBox_Quelle.Name = "comboBox_Quelle";
            comboBox_Quelle.Size = new Size(180, 23);
            comboBox_Quelle.TabIndex = 0;
            // 
            // button_OK
            // 
            button_OK.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            button_OK.Location = new Point(25, 145);
            button_OK.Name = "button_OK";
            button_OK.Size = new Size(75, 26);
            button_OK.TabIndex = 1;
            button_OK.Text = "OK";
            button_OK.UseVisualStyleBackColor = true;
            button_OK.Click += button_OK_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            label1.Location = new Point(25, 21);
            label1.Name = "label1";
            label1.Size = new Size(60, 17);
            label1.TabIndex = 2;
            label1.Text = "Das Teil: ";
            // 
            // label_Name
            // 
            label_Name.AutoSize = true;
            label_Name.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            label_Name.Location = new Point(87, 21);
            label_Name.Name = "label_Name";
            label_Name.Size = new Size(0, 17);
            label_Name.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            label2.Location = new Point(25, 75);
            label2.Name = "label2";
            label2.Size = new Size(385, 17);
            label2.TabIndex = 2;
            label2.Text = "Bitte wähle den passenden wert oder \"Cancel\" für's Ignorieren";
            // 
            // button_Cancel
            // 
            button_Cancel.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            button_Cancel.Location = new Point(130, 145);
            button_Cancel.Name = "button_Cancel";
            button_Cancel.Size = new Size(75, 26);
            button_Cancel.TabIndex = 1;
            button_Cancel.Text = "Cancel";
            button_Cancel.UseVisualStyleBackColor = true;
            button_Cancel.Click += button_Cancel_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            label3.Location = new Point(25, 48);
            label3.Name = "label3";
            label3.Size = new Size(151, 17);
            label3.TabIndex = 2;
            label3.Text = "Parameter wird erzeugt";
            // 
            // QuelleSelection
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(701, 195);
            Controls.Add(label_Name);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button_Cancel);
            Controls.Add(button_OK);
            Controls.Add(comboBox_Quelle);
            Name = "QuelleSelection";
            Text = "QuelleSelection";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBox_Quelle;
        private Button button_OK;
        private Label label1;
        public Label label_Name;
        private Label label2;
        private Button button_Cancel;
        private Label label3;
    }
}