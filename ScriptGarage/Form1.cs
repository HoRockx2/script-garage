using ScriptGarage.Common;

namespace ScriptGarage
{
    public partial class Form1 : Form
    {
        private ScriptFetcher _fetcher;

        public Form1()
        {
            InitializeComponent();

            _fetcher = new ScriptFetcher();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await _fetcher.Pooling();

            MessageBox.Show("Done");
        }
    }
}