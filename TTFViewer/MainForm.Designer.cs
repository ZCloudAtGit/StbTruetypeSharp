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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label5;
            this.codepointTextBox = new System.Windows.Forms.TextBox();
            this.ShowGlyphButton = new System.Windows.Forms.Button();
            this.bitmapPictureBox = new System.Windows.Forms.PictureBox();
            this.FontSelectorComboBox = new System.Windows.Forms.ComboBox();
            this.FontHeightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.BuildTypeTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.CodepointCountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.FirstCodepointTextBox = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.CharactersToPackTextBox = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.CharacterTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TextTextBox = new System.Windows.Forms.TextBox();
            this.ShowFontMapButton = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bitmapPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontHeightNumericUpDown)).BeginInit();
            this.BuildTypeTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CodepointCountNumericUpDown)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 10);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(95, 12);
            label2.TabIndex = 3;
            label2.Text = "First character";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(142, 10);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(95, 12);
            label3.TabIndex = 4;
            label3.Text = "Character count";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(4, 3);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(137, 12);
            label5.TabIndex = 2;
            label5.Text = "Character to be packed";
            // 
            // codepointTextBox
            // 
            this.codepointTextBox.Location = new System.Drawing.Point(8, 18);
            this.codepointTextBox.Name = "codepointTextBox";
            this.codepointTextBox.Size = new System.Drawing.Size(93, 21);
            this.codepointTextBox.TabIndex = 0;
            // 
            // ShowGlyphButton
            // 
            this.ShowGlyphButton.Location = new System.Drawing.Point(12, 154);
            this.ShowGlyphButton.Name = "ShowGlyphButton";
            this.ShowGlyphButton.Size = new System.Drawing.Size(248, 23);
            this.ShowGlyphButton.TabIndex = 1;
            this.ShowGlyphButton.Text = "Show";
            this.ShowGlyphButton.UseVisualStyleBackColor = true;
            this.ShowGlyphButton.Click += new System.EventHandler(this.ShowGlyphButton_Click);
            // 
            // bitmapPictureBox
            // 
            this.bitmapPictureBox.Location = new System.Drawing.Point(12, 200);
            this.bitmapPictureBox.Name = "bitmapPictureBox";
            this.bitmapPictureBox.Size = new System.Drawing.Size(100, 50);
            this.bitmapPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.bitmapPictureBox.TabIndex = 2;
            this.bitmapPictureBox.TabStop = false;
            // 
            // FontSelectorComboBox
            // 
            this.FontSelectorComboBox.FormattingEnabled = true;
            this.FontSelectorComboBox.Location = new System.Drawing.Point(12, 12);
            this.FontSelectorComboBox.Name = "FontSelectorComboBox";
            this.FontSelectorComboBox.Size = new System.Drawing.Size(121, 20);
            this.FontSelectorComboBox.TabIndex = 3;
            // 
            // FontHeightNumericUpDown
            // 
            this.FontHeightNumericUpDown.Location = new System.Drawing.Point(140, 11);
            this.FontHeightNumericUpDown.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.FontHeightNumericUpDown.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.FontHeightNumericUpDown.Name = "FontHeightNumericUpDown";
            this.FontHeightNumericUpDown.Size = new System.Drawing.Size(120, 21);
            this.FontHeightNumericUpDown.TabIndex = 4;
            this.FontHeightNumericUpDown.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.FontHeightNumericUpDown.ValueChanged += new System.EventHandler(this.FontHeightNumericUpDown_ValueChanged);
            // 
            // BuildTypeTabControl
            // 
            this.BuildTypeTabControl.Controls.Add(this.tabPage1);
            this.BuildTypeTabControl.Controls.Add(this.tabPage2);
            this.BuildTypeTabControl.Controls.Add(this.tabPage3);
            this.BuildTypeTabControl.Controls.Add(this.tabPage4);
            this.BuildTypeTabControl.Location = new System.Drawing.Point(12, 38);
            this.BuildTypeTabControl.Name = "BuildTypeTabControl";
            this.BuildTypeTabControl.SelectedIndex = 0;
            this.BuildTypeTabControl.Size = new System.Drawing.Size(248, 110);
            this.BuildTypeTabControl.TabIndex = 5;
            this.BuildTypeTabControl.SelectedIndexChanged += new System.EventHandler(this.BuildTypeTabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.codepointTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(240, 84);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Single";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "The character";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.CodepointCountNumericUpDown);
            this.tabPage2.Controls.Add(label3);
            this.tabPage2.Controls.Add(label2);
            this.tabPage2.Controls.Add(this.FirstCodepointTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(240, 84);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Range";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // CodepointCountNumericUpDown
            // 
            this.CodepointCountNumericUpDown.Location = new System.Drawing.Point(144, 25);
            this.CodepointCountNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.CodepointCountNumericUpDown.Name = "CodepointCountNumericUpDown";
            this.CodepointCountNumericUpDown.Size = new System.Drawing.Size(90, 21);
            this.CodepointCountNumericUpDown.TabIndex = 5;
            // 
            // FirstCodepointTextBox
            // 
            this.FirstCodepointTextBox.Location = new System.Drawing.Point(6, 25);
            this.FirstCodepointTextBox.Name = "FirstCodepointTextBox";
            this.FirstCodepointTextBox.Size = new System.Drawing.Size(90, 21);
            this.FirstCodepointTextBox.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.CharactersToPackTextBox);
            this.tabPage3.Controls.Add(label5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(240, 84);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Pack";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // CharactersToPackTextBox
            // 
            this.CharactersToPackTextBox.Location = new System.Drawing.Point(6, 18);
            this.CharactersToPackTextBox.Name = "CharactersToPackTextBox";
            this.CharactersToPackTextBox.Size = new System.Drawing.Size(228, 21);
            this.CharactersToPackTextBox.TabIndex = 3;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.ShowFontMapButton);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.CharacterTypeComboBox);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.TextTextBox);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(240, 84);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Text";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "Type of characters";
            // 
            // CharacterTypeComboBox
            // 
            this.CharacterTypeComboBox.FormattingEnabled = true;
            this.CharacterTypeComboBox.Location = new System.Drawing.Point(6, 19);
            this.CharacterTypeComboBox.Name = "CharacterTypeComboBox";
            this.CharacterTypeComboBox.Size = new System.Drawing.Size(121, 20);
            this.CharacterTypeComboBox.TabIndex = 6;
            this.CharacterTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.CharacterTypeComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "The text";
            // 
            // TextTextBox
            // 
            this.TextTextBox.Location = new System.Drawing.Point(6, 57);
            this.TextTextBox.Name = "TextTextBox";
            this.TextTextBox.Size = new System.Drawing.Size(93, 21);
            this.TextTextBox.TabIndex = 2;
            // 
            // ShowFontMapButton
            // 
            this.ShowFontMapButton.Location = new System.Drawing.Point(133, 17);
            this.ShowFontMapButton.Name = "ShowFontMapButton";
            this.ShowFontMapButton.Size = new System.Drawing.Size(101, 23);
            this.ShowFontMapButton.TabIndex = 8;
            this.ShowFontMapButton.Text = "Show font map";
            this.ShowFontMapButton.UseVisualStyleBackColor = true;
            this.ShowFontMapButton.Click += new System.EventHandler(this.ShowFontMapButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.BuildTypeTabControl);
            this.Controls.Add(this.FontHeightNumericUpDown);
            this.Controls.Add(this.FontSelectorComboBox);
            this.Controls.Add(this.bitmapPictureBox);
            this.Controls.Add(this.ShowGlyphButton);
            this.Name = "MainForm";
            this.Text = "TTFViewer";
            ((System.ComponentModel.ISupportInitialize)(this.bitmapPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FontHeightNumericUpDown)).EndInit();
            this.BuildTypeTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CodepointCountNumericUpDown)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox codepointTextBox;
        private System.Windows.Forms.Button ShowGlyphButton;
        private System.Windows.Forms.PictureBox bitmapPictureBox;
        private System.Windows.Forms.ComboBox FontSelectorComboBox;
        private System.Windows.Forms.NumericUpDown FontHeightNumericUpDown;
        private System.Windows.Forms.TabControl BuildTypeTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FirstCodepointTextBox;
        private System.Windows.Forms.NumericUpDown CodepointCountNumericUpDown;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox CharactersToPackTextBox;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CharacterTypeComboBox;
        private System.Windows.Forms.Button ShowFontMapButton;
    }
}

