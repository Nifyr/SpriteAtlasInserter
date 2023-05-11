namespace SpriteAtlasInserter
{
    partial class TextInputForm
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
            tableLayoutPanel1 = new TableLayoutPanel();
            numericUpDown1 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            label = new Label();
            richTextBox = new RichTextBox();
            confirmButton = new Button();
            label1 = new Label();
            label2 = new Label();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(numericUpDown1, 1, 2);
            tableLayoutPanel1.Controls.Add(numericUpDown2, 3, 2);
            tableLayoutPanel1.Controls.Add(label, 0, 0);
            tableLayoutPanel1.Controls.Add(richTextBox, 0, 1);
            tableLayoutPanel1.Controls.Add(confirmButton, 3, 3);
            tableLayoutPanel1.Controls.Add(label1, 0, 2);
            tableLayoutPanel1.Controls.Add(label2, 2, 2);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(401, 190);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numericUpDown1.Location = new Point(43, 116);
            numericUpDown1.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(154, 27);
            numericUpDown1.TabIndex = 7;
            numericUpDown1.Value = new decimal(new int[] { 128, 0, 0, 0 });
            numericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numericUpDown2.Location = new Point(243, 116);
            numericUpDown2.Maximum = new decimal(new int[] { int.MinValue, 0, 0, 0 });
            numericUpDown2.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(155, 27);
            numericUpDown2.TabIndex = 6;
            numericUpDown2.Value = new decimal(new int[] { 128, 0, 0, 0 });
            numericUpDown2.ValueChanged += NumericUpDown1_ValueChanged;
            // 
            // label
            // 
            label.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(label, 4);
            label.Location = new Point(3, 0);
            label.Name = "label";
            label.Size = new Size(50, 20);
            label.TabIndex = 0;
            label.Text = "label1";
            // 
            // richTextBox
            // 
            tableLayoutPanel1.SetColumnSpan(richTextBox, 4);
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.Location = new Point(3, 33);
            richTextBox.Name = "richTextBox";
            richTextBox.Size = new Size(395, 74);
            richTextBox.TabIndex = 1;
            richTextBox.Text = "";
            richTextBox.TextChanged += OutTextChanged;
            // 
            // confirmButton
            // 
            confirmButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            confirmButton.Location = new Point(298, 158);
            confirmButton.Name = "confirmButton";
            confirmButton.Size = new Size(100, 29);
            confirmButton.TabIndex = 2;
            confirmButton.Text = "Confirm";
            confirmButton.UseVisualStyleBackColor = true;
            confirmButton.Click += ConfirmClick;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(16, 120);
            label1.Name = "label1";
            label1.Size = new Size(21, 20);
            label1.TabIndex = 3;
            label1.Text = "X:";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(217, 120);
            label2.Name = "label2";
            label2.Size = new Size(20, 20);
            label2.TabIndex = 4;
            label2.Text = "Y:";
            // 
            // TextInputForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(425, 214);
            Controls.Add(tableLayoutPanel1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TextInputForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TextInputForm";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label;
        private RichTextBox richTextBox;
        private Button confirmButton;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private Label label1;
        private Label label2;
    }
}