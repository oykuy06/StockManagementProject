namespace StockManagement
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F);
            this.button3.Location = new System.Drawing.Point(678, 359);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(215, 90);
            this.button3.TabIndex = 0;
            this.button3.Text = "Login";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F);
            this.label1.Location = new System.Drawing.Point(385, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(919, 55);
            this.label1.TabIndex = 1;
            this.label1.Text = "Welcome To Stock Management Program";
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1499, 583);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Name = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
    }
}
