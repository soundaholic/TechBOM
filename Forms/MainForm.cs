using MECMOD;
using ProductStructureTypeLib;
using Application = INFITF.Application;
using System.IO;
using TechBOM.Interfaces;
using TechBOM.SingleNodeDomain;

namespace TechBOM
{
    public partial class MainForm : CustomMetroForm
    {
        private readonly ICatiaConnect _catiaConnect;
        private readonly ICounter _counter;
        private readonly Func<RootNode> _rootNodeFactory;  // Factory for RootNode
        private readonly Func<ExcelProcessor> _excelProcessorFactory;  // Factory for ExcelProcessor

        private RootNode _rootNode;  // Store the current RootNode instance
        private ExcelProcessor _excelProcessor;  // Store the current ExcelProcessor instance
        private CatiaProcessor _catiaProcessor;

        private Application _catia = CatiaConnect.Instance.ConnectCatia();
        private string _saveFilePathFormData = string.Empty;
        private string _TemplateFilePath = string.Empty;
        private List<PartDocument> _productionParts = [];

        //private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public MainForm(ICatiaConnect catiaConnect, Func<ExcelProcessor> excelProcessorFactory, ICounter counter, Func<RootNode> rootNodeFactory)
        {
            _catiaConnect = catiaConnect;
            _excelProcessorFactory = excelProcessorFactory;  // Injected factory
            _counter = counter;
            _rootNodeFactory = rootNodeFactory;

            // Initialize the first RootNode and ExcelProcessor instance
            _rootNode = _rootNodeFactory();
            _excelProcessor = _excelProcessorFactory();  // Initialize the first ExcelProcessor

            InitializeComponent();
            InitializeLanguageComboBox();
            ComboBoxLanguage_SelectedIndexChanged(null, null);
        }


        private bool IsTechnologie()
        {
            string rn = _rootNode.RootPartNumber;
            string thirdChar = rn.Substring(2, 1);

            if (thirdChar == "_")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_rootNode.ActiveDoc is ProductDocument)
            {
                _saveFilePathFormData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "formData.txt");

                if (IsTechnologie())
                {
                    RefreshFormData();

                    LoadFormData();

                    try
                    {
                        _TemplateFilePath = _excelProcessor.TemplateFilePath;
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, "ArgumentException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close();
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        MessageBox.Show(ex.Message, "DirectoryNotFoundException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("This Tool is for Technologie Design only\n" +
                        "Name like: \"RF_..., ST_..., LK_... etc.\"\n" +
                        "For FFT-Standard Data use \"BOM-Exporter\"", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Close();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please open a product document.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
        }

        private async void Button_Start_Click(object sender, EventArgs e)
        {
            _catiaProcessor = new CatiaProcessor();

            if (Convert.ToInt32(textBox_LastCollumnLength.Text) > 255)
            {
                MessageBox.Show("Maximale Länge darf nicht größer als 255 Zeichen sein");
                return;
            }

            // Reset the Counter
            Counter.Instance.Reset();  // Ensure the counter is reset at the start

            _rootNode = _rootNodeFactory();  // Create a new RootNode instance
            _excelProcessor = _excelProcessorFactory();  // Create a new ExcelProcessor instance

            RefreshFormData();
            _productionParts.Clear();

            _catiaProcessor.CatHelperReset();

            if (File.Exists(_rootNode.SaveFileName))
            {
                DialogResult result = MessageBox.Show("File already exists. Do you want to overwrite it?", "File exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool isSaved = false;

                    while (!isSaved)
                    {
                        try
                        {
                            File.Copy(_excelProcessor.TemplateFilePath, _rootNode.SaveFileName, overwrite: true);
                            isSaved = true;
                        }
                        catch (IOException)
                        {
                            string owner = _excelProcessor.GetFileOwner(_rootNode.SaveFileName);
                            result = MessageBox.Show($"The file is currently opened by {owner}. Please close the file and try again.", "File in use", MessageBoxButtons.OKCancel);

                            if (result == DialogResult.Cancel)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                File.Copy(_excelProcessor.TemplateFilePath, _rootNode.SaveFileName, overwrite: true);
            }

            // Initialize Progress reporting
            var progress = new Progress<(int, string)>(ReportProgress);

            // Start the asynchronous process
            await RunBomProcessingAsync(progress);

            if (Counter.CountOfCoruptedParts != 0) 
            {
                MessageBox.Show
                    (
                        $"Some links are broken. Please check the CATIA Structure\nCount of broken links: {Counter.CountOfCoruptedParts}",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly
                    );
            }
        }

        private async Task RunBomProcessingAsync(IProgress<(int, string)> progress)
        {
            try
            {
                progress.Report((0, "Starting BOM processing..."));

                await Task.Run(() =>
                {
                    var productDoc = (ProductDocument)_rootNode.ActiveDoc;
                    var product = productDoc.Product;

                    // Step 1: Refresh form data
                    progress.Report((10, "Refreshing form data..."));
                    RefreshFormData();

                    // Step 2: Fill head data in Excel
                    progress.Report((20, "Filling head data..."));
                    FillExcelHeadData();

                    // Step 3: Count parts asynchronously
                    progress.Report((30, "Counting parts..."));
                    Invoke((MethodInvoker)(() =>
                    {
                        _counter.Reset();
                        Counter.Instance.CountParts(product, 0, comboBox_ScanDepth.Text);
                    }));

                    // Step 4: Process BOM entries asynchronously
                    progress.Report((50, "Processing BOM entries..."));
                    ProcessBomEntry();

                    // Step 5: Adjust column width
                    progress.Report((90, "Adjusting column width..."));
                    for (int i = 0; i <= 15; i++)
                    {
                        _excelProcessor.WorkSheet.AutoSizeColumn(i);

                    }

                    for (int i = 0; i < 500; i++)
                    {
                        _excelProcessor.WorkSheet.AutoSizeRow(i);
                    }

                    _excelProcessor.WorkSheet.SetColumnWidth(16, Convert.ToInt32(textBox_LastCollumnLength.Text) * 256);
                    
                    // Step 6: Save the Excel file
                    progress.Report((95, "Saving Excel document..."));
                    _excelProcessor.SaveExcelDocument(_rootNode.SaveFileName);
                });

                // After task completion, open the saved file
                progress.Report((100, "Opening Excel file..."));
                _excelProcessor.OpenFile(_rootNode.SaveFileName);

                // UI Update: Completed
                lbl_Status.Text = "Done!";
            }
            catch (Exception ex)
            {
                // Handle errors
                lbl_Status.Text = $"Error: {ex.Message}";
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillExcelHeadData()
        {
            if (_rootNode.ActiveDoc is ProductDocument oProductDoc)
            {
                SingleNode singleNode = new(oProductDoc);

                ExcelHeadData headData = new()
                {
                    Customer = textBox_Customer.Text,
                    Oem = textBox_Oem.Text,
                    Project = textBox_Project.Text,
                    Plant = textBox_Plant.Text,
                    Line = textBox_Line.Text,
                    Station = textBox_Station.Text,
                    Version = textBox_Version.Text
                };

                Invoke((MethodInvoker)delegate
                {
                    _excelProcessor.FillExcelHeadData(singleNode, headData, _productionParts);
                });
            }
            else if (_rootNode.ActiveDoc is PartDocument oPartDoc)
            {
                _productionParts.Add(oPartDoc);
            }
        }

        private void ProcessBomEntry()
        {
            _catiaProcessor.CatHelperReset();

            if (_rootNode.ActiveDoc is ProductDocument oProductDoc)
            {
                Product oRootProd = oProductDoc.Product;

                string rootPartNumber = oRootProd.get_Name();

                var bomEntry = new BomEntry(_excelProcessor.WorkSheet);
                var depth = string.Empty;

                Invoke((MethodInvoker)delegate
                {
                    depth = comboBox_ScanDepth.Text;
                });

                // Walk down the tree and fill the BOM
                _catiaProcessor.WalkDownTree(oRootProd, 0, depth);

                // Order by PosNumber
                var orderedNodes = from s in _catiaProcessor.Nodes
                                   orderby s.Data.PosNumber ascending
                                   select s;
                _catiaProcessor.Nodes = orderedNodes.ToList();

                int totalNodes = _catiaProcessor.Nodes.Count;
                int i = 0;

                foreach (var node in orderedNodes)
                {
                    bomEntry.FillBomValues(node, i);
                    i++;
                }
            }
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_rootNode.ActiveDoc is ProductDocument oProductDoc)
            {
                SaveFormData();
            }
        }

        private void InitializeLanguageComboBox()
        {
            comboBox_Language.SelectedIndex = 1;
            comboBox_Language.SelectedIndexChanged += ComboBoxLanguage_SelectedIndexChanged;
        }

        private void ComboBoxLanguage_SelectedIndexChanged(object? sender, EventArgs? e)
        {
            switch (comboBox_Language.SelectedItem!.ToString())
            {
                case "English":
                    lbl_ScanDepth.Text = "Scan Depth:";
                    lbl_Customer.Text = "Customer:";
                    lbl_Project.Text = "Project:";
                    lbl_Date.Text = "Date:";
                    lbl_Plant.Text = "Plant:";
                    lbl_Line.Text = "Line:";
                    lbl_DrawingNumber.Text = "Drawing Number:";
                    groupBox_HeadData.Text = "Head Data";
                    lbl_Language.Text = "Language";
                    break;

                case "German":
                    lbl_ScanDepth.Text = "Scanntiefe:";
                    lbl_Customer.Text = "Kunde:";
                    lbl_Project.Text = "Projekt:";
                    lbl_Date.Text = "Datum:";
                    lbl_Plant.Text = "Werk:";
                    lbl_Line.Text = "Linie:";
                    lbl_DrawingNumber.Text = "Zeichnunsnummer:";
                    groupBox_HeadData.Text = "Kopfdaten";
                    lbl_Language.Text = "Sprache";
                    break;
            }
        }

        private void groupBox_HeadData_Enter(object sender, EventArgs e)
        {

        }

        private void RefreshFormData()
        {
            Invoke((MethodInvoker)delegate
            {
                textBox_Name.Text = _rootNode.NameForTextBox;
                textBox_DrawingNumber.Text = _rootNode.DrawingNumberForTextBox;
                textBox_Version.Text = _rootNode.VersionForTextBox;
                textBox_Date.Text = DateTime.Now.ToString("dd.MM.yyyy");
            });
        }

        private void SaveFormData()
        {
            using StreamWriter writer = new(_saveFilePathFormData);
            writer.WriteLine(textBox_Customer.Text);
            writer.WriteLine(textBox_Oem.Text);
            writer.WriteLine(textBox_Project.Text);
            writer.WriteLine(textBox_Plant.Text);
            writer.WriteLine(textBox_Line.Text);
            writer.WriteLine(textBox_Station.Text);
        }

        private void LoadFormData()
        {
            if (File.Exists(_saveFilePathFormData))
            {
                using StreamReader reader = new(_saveFilePathFormData);
                textBox_Customer.Text = reader.ReadLine();
                textBox_Oem.Text = reader.ReadLine();
                textBox_Project.Text = reader.ReadLine();
                textBox_Plant.Text = reader.ReadLine();
                textBox_Line.Text = reader.ReadLine();
                textBox_Station.Text = reader.ReadLine();
            }
            comboBox_ScanDepth.SelectedIndex = 5;
        }

        private void ReportProgress((int, string) progressData)
        {
            var (percent, message) = progressData;
            lbl_Status.Text = $"{message} ({percent}%)";
            progressBar.Value = percent;
        }
    }
}
