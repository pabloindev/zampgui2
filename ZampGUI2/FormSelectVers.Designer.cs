namespace ZampGUI2
{
    partial class FormSelectVers
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
            btnCancel = new Button();
            btnOK = new Button();
            label1 = new Label();
            comboBoxVersion = new ComboBox();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(149, 107);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(117, 34);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOK
            // 
            btnOK.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Location = new Point(26, 107);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(117, 34);
            btnOK.TabIndex = 9;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 24);
            label1.Name = "label1";
            label1.Size = new Size(263, 17);
            label1.TabIndex = 8;
            label1.Text = "Select your version for Apache and Php";
            // 
            // comboBoxVersion
            // 
            comboBoxVersion.BackColor = Color.FromArgb(45, 45, 48);
            comboBoxVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxVersion.ForeColor = Color.White;
            comboBoxVersion.FormattingEnabled = true;
            comboBoxVersion.Location = new Point(26, 63);
            comboBoxVersion.Name = "comboBoxVersion";
            comboBoxVersion.Size = new Size(240, 25);
            comboBoxVersion.TabIndex = 11;
            comboBoxVersion.DrawItem += comboBoxVersion_DrawItem;
            // 
            // FormSelectVers
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(289, 168);
            Controls.Add(comboBoxVersion);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(label1);
            Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            MaximumSize = new Size(305, 207);
            MinimumSize = new Size(305, 207);
            Name = "FormSelectVers";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FormSelectVers";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private Label label1;
        private ComboBox comboBoxVersion;
    }
}