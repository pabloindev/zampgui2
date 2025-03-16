namespace ZampGUI2
{
    partial class FormAsk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAsk));
            label1 = new Label();
            btnOK = new Button();
            btnCancel = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(34, 27);
            label1.Name = "label1";
            label1.Size = new Size(46, 17);
            label1.TabIndex = 1;
            label1.Text = "label1";
            // 
            // btnOK
            // 
            btnOK.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Location = new Point(140, 104);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(117, 34);
            btnOK.TabIndex = 5;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(64, 64, 64);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(263, 104);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(117, 34);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(45, 45, 48);
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(34, 62);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(451, 25);
            textBox1.TabIndex = 7;
            // 
            // FormAsk
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 45, 48);
            ClientSize = new Size(515, 161);
            Controls.Add(textBox1);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(label1);
            Font = new Font("Arial", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.White;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormAsk";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FormAsk";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Button btnOK;
        private Button btnCancel;
        private TextBox textBox1;
    }
}