namespace TechBOM.Forms
{
    public partial class QuelleSelection : Form
    {
        public string SelectedValue { get; private set; }

        public QuelleSelection(object[] options)
        {
            InitializeComponent();
            foreach (var option in options)
            {
                comboBox_Quelle.Items.Add(option);
            }
            comboBox_Quelle.SelectedIndex = 0; // Выбираем первый элемент по умолчанию
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            SelectedValue = comboBox_Quelle.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
