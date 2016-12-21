namespace WindowsFormsApplication3
{
    partial class Form2
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
            this.statusBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // statusBox2
            // 
            this.statusBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusBox2.Font = new System.Drawing.Font("Lucida Console", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBox2.Location = new System.Drawing.Point(17, 16);
            this.statusBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.statusBox2.Multiline = true;
            this.statusBox2.Name = "statusBox2";
            this.statusBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.statusBox2.Size = new System.Drawing.Size(1012, 907);
            this.statusBox2.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 938);
            this.Controls.Add(this.statusBox2);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(1061, 974);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox statusBox2;
    }
}