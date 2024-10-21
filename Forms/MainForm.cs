using MECMOD;
using ProductStructureTypeLib;
using Application = INFITF.Application;
using System.ComponentModel;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace TechBOM
{
    public partial class MainForm : CustomMetroForm
    {
        private RootNode _rootNode;
        private ExcelProcessor _excelProcessor;
        private Application _catia = CatiaConnect.Instance.ConnectCatia();
        private string _saveFilePathFormData = string.Empty;
        private string _TemplateFilePath = string.Empty;
        private List<PartDocument> _productionParts = [];
        
        //private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private BackgroundWorker _backgroundWorker;

        public MainForm()
        {
            InitializeComponent();
            InitializeLanguageComboBox();
            ComboBoxLanguage_SelectedIndexChanged(null, null);
            _rootNode = new RootNode();
            _excelProcessor = new ExcelProcessor(_rootNode);
            _backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Counter counter = Counter.Instance;

            ProductDocument productDoc = (ProductDocument)_rootNode.ActiveDoc;
            Product product = productDoc.Product;

            _backgroundWorker.ReportProgress(0);

            _backgroundWorker.ReportProgress(10, "Getting head parameters...");
            RefreshFormData();

            _backgroundWorker.ReportProgress(20, "Filling head data...");
            FillExcelHeadData();

            _backgroundWorker.ReportProgress(30, "Counting parts...");
            Invoke((MethodInvoker)delegate
            {
                counter.CountParts(product, 0, comboBox_ScanDepth.Text);
            });

            _backgroundWorker.ReportProgress(50, "Processing BOM entries...");
            ProcessBomEntry();

            _backgroundWorker.ReportProgress(90, "Adjusting of columns width...");
            for (int i = 0; i < 500; i++)
            {
                _excelProcessor.WorkSheet.AutoSizeColumn(i);
            }
            for (int i = 0; i < 500; i++)
            {
                _excelProcessor.WorkSheet.AutoSizeRow(i);
            }

            _backgroundWorker.ReportProgress(100, "Saving BOM entries...");
            _excelProcessor.SaveExcelDocument(_rootNode.SaveFileName);
            _excelProcessor.OpenFile(_rootNode.SaveFileName);
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            string? statusMessage = e.UserState as string;
            lbl_Status.Text = $"{statusMessage} ({progress}%)";
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbl_Status.Text = "Done!";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_rootNode.ActiveDoc is ProductDocument)
            {
                _rootNode = new();
                _excelProcessor = new(_rootNode);
                _saveFilePathFormData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "formData.txt");

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
                MessageBox.Show("Please open a product document.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            _rootNode = new();
            _excelProcessor = new(_rootNode);

            Counter.Instance.Reset();
            _rootNode = new RootNode();
            RefreshFormData();
            _productionParts.Clear();
            CatiaProcessor.Instance.CatHelperReset();

            if (System.IO.File.Exists(_rootNode.SaveFileName))
            {
                DialogResult result = MessageBox.Show("File already exists. Do you want to overwrite it?", "File exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    bool isSaved = false;

                    while (!isSaved)
                    {
                        try
                        {
                            System.IO.File.Copy(_excelProcessor.TemplateFilePath, _rootNode.SaveFileName, overwrite: true);
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
                System.IO.File.Copy(_excelProcessor.TemplateFilePath, _rootNode.SaveFileName, overwrite: true);
            }

            _backgroundWorker.RunWorkerAsync();
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
            CatiaProcessor.Instance.CatHelperReset();

            if (_rootNode.ActiveDoc is ProductDocument oProductDoc)
            {
                Product oRootProd = oProductDoc.Product;
                var bomEntry = new BomEntry(_excelProcessor.WorkSheet);
                var depth = string.Empty;
                
                Invoke((MethodInvoker)delegate
                {
                    depth = comboBox_ScanDepth.Text;
                });

                // Walk down the tree and fill the BOM
                CatiaProcessor.Instance.WalkDownTree(oRootProd, 0 , depth);

                // Order by PosNumber
                var orderedNodes = from s in CatiaProcessor.Instance.Nodes
                                   orderby s.PosNumber ascending
                                   select s;
                CatiaProcessor.Instance.Nodes = orderedNodes.ToList();

                int totalNodes = CatiaProcessor.Instance.Nodes.Count;
                int i = 0;

                foreach (var node in orderedNodes)
                {
                    bomEntry.FillBomValues(node, i);
                    i++;

                    int progress = 40 + (i * 50 / totalNodes);
                    _backgroundWorker.ReportProgress(progress);
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
            if (System.IO.File.Exists(_saveFilePathFormData))
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
    }
}
