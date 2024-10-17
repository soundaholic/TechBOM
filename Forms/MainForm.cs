using INFITF;
using MECMOD;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ProductStructureTypeLib;
using Application = INFITF.Application;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TechBOM
{
    public partial class MainForm : CustomMetroForm
    {
        private static Application _catia = CatiaConnect.Instance.ConnectCatia();
        private static Document _oRootDocument = _catia.ActiveDocument;
        private readonly ExcelHelper _excelHelper = new();

        private IWorkbook _workbook;
        private ISheet _workSheet;
        private string _savePath;
        private string _saveFileName;
        private string _saveFilePathFormData;
        private string _rootPartNumber;
        private string _filePathTemplate;
        
        private List<PartDocument> _productionParts = [];

        //private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private BackgroundWorker _backgroundWorker;

        public MainForm()
        {
            InitializeComponent();
            InitializeLanguageComboBox();
            ComboBoxLanguage_SelectedIndexChanged(null, null);
        }

        private string GetTemplatePath()
        {
            // create configuration for reading appSettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            string relativePath = configuration["FilePaths:TemplatePath"]!;

            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentException("Template path is empty");
            }

            var file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
            if(!file.Exists)
            {
                throw new DirectoryNotFoundException("Template file does not exist");
            }

            return file.FullName;
        }

        private void InitializeBackgroundWorker()
        {
            _backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };

            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void CountingParts()
        {
            if (_oRootDocument is ProductDocument oProductDoc)
            {
                Counter counter = Counter.Instance;
                var depth = string.Empty;

                //reset the counter
                counter.Reset();

                _backgroundWorker.ReportProgress(0, "Counting parts...");

                Invoke((MethodInvoker)delegate
                {
                    depth = comboBox_ScanDepth.Text;
                });

                counter.CountParts(oProductDoc.Product, 0, depth);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _backgroundWorker.ReportProgress(0);

            _backgroundWorker.ReportProgress(10, "Getting head parameters...");
            GetHeadParameters();

            _backgroundWorker.ReportProgress(20, "Filling head data...");
            FillHeadData();

            _backgroundWorker.ReportProgress(30, "Counting parts...");
            CountingParts();

            _backgroundWorker.ReportProgress(50, "Processing BOM entries...");
            ProcessBomEntry();

            _backgroundWorker.ReportProgress(90, "Adjusting of columns width...");
            for (int i = 0; i < 500; i++)
            {
                _workSheet.AutoSizeColumn(i);
            }
            for (int i = 0; i < 500; i++)
            {
                _workSheet.AutoSizeRow(i);
            }

            _backgroundWorker.ReportProgress(100, "Saving BOM entries...");
            SaveExcelDocument(_saveFileName);

            _excelHelper.OpenFile(_saveFileName);
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

        private void SaveFormData()
        {
            using StreamWriter writer = new(_saveFilePathFormData);
            writer.WriteLine(textBox_Customer.Text);
            writer.WriteLine(textBox_Oem.Text);
            writer.WriteLine(textBox_Project.Text);
        }

        private void LoadFormData()
        {
            if (System.IO.File.Exists(_saveFilePathFormData))
            {
                using StreamReader reader = new(_saveFilePathFormData);
                textBox_Customer.Text = reader.ReadLine();
                textBox_Oem.Text = reader.ReadLine();
                textBox_Project.Text = reader.ReadLine();
            }
            comboBox_ScanDepth.SelectedIndex = 5;
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            //_savePath = string.Empty;
            //_rootPartNumber = string.Empty;

            _oRootDocument = _catia.ActiveDocument;

            GetHeadParameters();

            InitializeBackgroundWorker();
            _productionParts.Clear();
            CatiaHelper.Instance.CatHelperReset();
            _saveFileName = Path.Combine( _savePath,$"{_rootPartNumber}.xlsx");

            if (System.IO.File.Exists(_saveFileName))
            {
                DialogResult result = MessageBox.Show("File already exists. Do you want to overwrite it?", "File exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    bool isSaved = false;

                    while (!isSaved)
                    {
                        try
                        {
                            System.IO.File.Copy(_filePathTemplate, _saveFileName, overwrite: true);
                            isSaved = true;
                        }
                        catch (IOException)
                        {
                            string owner = GetFileOwner(_saveFileName);
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
                System.IO.File.Copy(_filePathTemplate, _saveFileName, overwrite: true);
            }
            
            using (FileStream file = new(_saveFileName, FileMode.Open, FileAccess.ReadWrite))
            {
                _workbook = new XSSFWorkbook(file); 
            }
            
            _backgroundWorker.RunWorkerAsync();
        }

        private void GetHeadParameters()
        {
            if (_oRootDocument is ProductDocument oProductDoc)
            {
                _savePath = oProductDoc.Path;
                _rootPartNumber = oProductDoc.Product.get_Name();
            }

            SingleNode singleNode = new(_oRootDocument);

            // Use Invoke to update UI controls from a non-UI thread
            Invoke((MethodInvoker)delegate
            {
                textBox_Name.Text = singleNode.Name;
                textBox_DrawingNumber.Text = singleNode.DrawingNumber;
                textBox_Version.Text = singleNode.Revision;
                textBox_Date.Text = DateTime.Now.ToString("dd.MM.yyyy");
            });
        }

        private void FillHeadData()
        {
            DateTime dateTime = DateTime.Now;

            if (_oRootDocument is ProductDocument oProductDoc)
            {
                SingleNode singleNode = new(oProductDoc);

                _workSheet = _workbook.GetSheet("bom");

                if (singleNode.IsZsb)
                {
                    IRow oRowPartNumber = _workSheet.GetRow(5);
                    ICell oCellPartNumber = oRowPartNumber.GetCell(7);
                    oCellPartNumber.SetCellValue(singleNode.Name);

                    IRow oRootOrderNumber = _workSheet.GetRow(6);
                    ICell oCellRootOrderNumber = oRootOrderNumber.GetCell(7);
                    oCellRootOrderNumber.SetCellValue(singleNode.DrawingNumber);
                }

                Invoke((MethodInvoker)delegate
                {
                    IRow oRowCustomer = _workSheet.GetRow(2);
                    ICell oCellCustomer = oRowCustomer.GetCell(3);
                    oCellCustomer.SetCellValue(textBox_Customer.Text);

                    IRow oRowOem = _workSheet.GetRow(3);
                    ICell oCellOem = oRowOem.GetCell(3);
                    oCellOem.SetCellValue(textBox_Oem.Text);

                    IRow oRowProject = _workSheet.GetRow(4);
                    ICell oCellProject = oRowProject.GetCell(3);
                    oCellProject.SetCellValue(textBox_Project.Text);

                    string formattedDate = dateTime.ToString("dd.MM.yyyy");
                    IRow oRowDate = _workSheet.GetRow(7);
                    ICell oCellDate = oRowDate.GetCell(3);
                    oCellDate.SetCellValue(formattedDate);

                    IRow oRowPlant = _workSheet.GetRow(2);
                    ICell oCellPlant = oRowPlant.GetCell(7);
                    oCellPlant.SetCellValue(textBox_Plant.Text);

                    IRow oRowLine = _workSheet.GetRow(3);
                    ICell oCellLine = oRowLine.GetCell(7);
                    oCellLine.SetCellValue(textBox_Line.Text);

                    IRow oRowStation = _workSheet.GetRow(4);
                    ICell oCellStation = oRowStation.GetCell(7);
                    oCellStation.SetCellValue(textBox_Station.Text);

                    IRow oRowRevision = _workSheet.GetRow(7);
                    ICell oCellRevision = oRowRevision.GetCell(7);
                    oCellRevision.SetCellValue(textBox_Version.Text);
                });
            }
            else if (_oRootDocument is PartDocument oPartDoc)
            {
                _productionParts.Add(oPartDoc);
            }
        }

        private void ProcessBomEntry()
        {
            CatiaHelper.Instance.CatHelperReset();

            if (_oRootDocument is ProductDocument oProductDoc)
            {
                Product oRootProd = oProductDoc.Product;
                var bomEntry = new BomEntry(_workSheet);
                var depth = string.Empty;
                
                Invoke((MethodInvoker)delegate
                {
                    depth = comboBox_ScanDepth.Text;
                });

                // Walk down the tree and fill the BOM
                CatiaHelper.Instance.WalkDownTree(oRootProd, 0 , depth);

                // Order by PosNumber
                var orderedNodes = from s in CatiaHelper.Instance.Nodes
                                   orderby s.PosNumber ascending
                                   select s;
                CatiaHelper.Instance.Nodes = orderedNodes.ToList();

                int totalNodes = CatiaHelper.Instance.Nodes.Count;
                int i = 0;

                foreach (var node in CatiaHelper.Instance.Nodes)
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

        private static string GetFileOwner(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                var fileSecurity = fileInfo.GetAccessControl();
                var identity = fileSecurity.GetOwner(typeof(System.Security.Principal.NTAccount));
                if (identity != null)
                {
                    return identity.ToString();
                }
                else
                {
                    return "Unknown user";
                }
            }
            catch (Exception)
            {
                return "Unknown user";
            }
        }

        private void SaveExcelDocument(string savePath)
        {
            bool isSaved = false;

            while (!isSaved)
            {
                try
                {
                    using (var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                    {
                        _workbook.Write(fs);
                        isSaved = true;
                    }
                }
                catch (IOException)
                {
                    string owner = GetFileOwner(savePath);
                    DialogResult result = MessageBox.Show($"The file is currently opened by {owner}. Please close the file and try again.", "File in use", MessageBoxButtons.OKCancel);

                    if (result == DialogResult.Cancel)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                    break;
                }
            }

            if (isSaved)
            {
                MessageBox.Show("File saved successfully.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _saveFilePathFormData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "formData.txt");

            GetHeadParameters();
            LoadFormData();
            try
            {
                _filePathTemplate = GetTemplatePath();
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFormData();
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
    }
}
