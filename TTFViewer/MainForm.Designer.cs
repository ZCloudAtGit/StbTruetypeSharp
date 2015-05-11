namespace TTFViewer
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
            this.codepointTextBox = new System.Windows.Forms.TextBox();
            this.ShowGlyphButton = new System.Windows.Forms.Button();
            this.bitmapPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.bitmapPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // codepointTextBox
            // 
            this.codepointTextBox.Location = new System.Drawing.Point(12, 12);
            this.codepointTextBox.Name = "codepointTextBox";
            this.codepointTextBox.Size = new System.Drawing.Size(100, 21);
            this.codepointTextBox.TabIndex = 0;
            // 
            // ShowGlyphButton
            // 
            this.ShowGlyphButton.Location = new System.Drawing.Point(118, 10);
            this.ShowGlyphButton.Name = "ShowGlyphButton";
            this.ShowGlyphButton.Size = new System.Drawing.Size(75, 23);
            this.ShowGlyphButton.TabIndex = 1;
            this.ShowGlyphButton.Text = "Show";
            this.ShowGlyphButton.UseVisualStyleBackColor = true;
            this.ShowGlyphButton.Click += new System.EventHandler(this.ShowGlyphButton_Click);
            // 
            // bitmapPictureBox
            // 
            this.bitmapPictureBox.Location = new System.Drawing.Point(12, 39);
            this.bitmapPictureBox.Name = "bitmapPictureBox";
            this.bitmapPictureBox.Size = new System.Drawing.Size(100, 50);
            this.bitmapPictureBox.TabIndex = 2;
            this.bitmapPictureBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.bitmapPictureBox);
            this.Controls.Add(this.ShowGlyphButton);
            this.Controls.Add(this.codepointTextBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.bitmapPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox codepointTextBox;
        private System.Windows.Forms.Button ShowGlyphButton;
        private System.Windows.Forms.PictureBox bitmapPictureBox;
    }
}

