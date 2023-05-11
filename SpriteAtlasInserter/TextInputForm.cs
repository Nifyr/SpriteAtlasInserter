namespace SpriteAtlasInserter
{
    public partial class TextInputForm : Form
    {
        private string _outString;
        public string OutString { get { return _outString; } }

        private Size _outSize;
        public Size OutSize { get { return _outSize; } }

        public TextInputForm(string caption, string message, string defaultInput, Size defaultSize)
        {
            InitializeComponent();

            Text = caption;
            label.Text = message;
            _outString = defaultInput;
            _outSize = defaultSize;
            richTextBox.Text = defaultInput;
            numericUpDown1.Value = defaultSize.Width;
            numericUpDown2.Value = defaultSize.Height;
        }

        private void OutTextChanged(object sender, EventArgs e)
        {
            _outString = richTextBox.Text;
        }

        private void ConfirmClick(object sender, EventArgs e)
        {
            Close();
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _outSize = new((int)numericUpDown1.Value, (int)numericUpDown2.Value);
        }
    }
}
