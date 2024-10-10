using INFITF;
using MECMOD;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ProductStructureTypeLib;
using System.Runtime.InteropServices;
using System.Text;
using Application = INFITF.Application;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace TechBOM
{
    public partial class MainForm : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);

        private Document _oRootDocument;
        private readonly Application _catia = CatiaConnect.Instance.ConnectCatia();
        private readonly ExcelHelper _excelHelper = new ExcelHelper();

        private BomEntry _bomEntry;
        private readonly IWorkbook _workBook;
        private ISheet _workSheet;

        private readonly DateTime _dateTime = DateTime.Now;

        private string _savePath;
        private string _saveFileName;

        private string _selectedDepth;

        private readonly string saveFilePath;
        private string _rootPartNumber;
        private readonly string _filePathTemplate;
        private readonly string _currentNodeName = string.Empty;
        
        private readonly List<PartDocument> _productionParts = new List<PartDocument>();

        //private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private BackgroundWorker _backgroundWorker;

        public MainForm()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            InitializeLanguageComboBox();
            ComboBoxLanguage_SelectedIndexChanged(null, null);
            _filePathTemplate = GetTemplatePath();
            _workBook = new XSSFWorkbook(_filePathTemplate);
            _oRootDocument = _catia.ActiveDocument;
            saveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "formData.txt");
            LoadFormData();
        }

        private string GetTemplatePath()
        {
            // Создаем конфигурацию для чтения из appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Корневая папка приложения
                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            // Получаем относительный путь из конфигурации
            string relativePath = configuration["FilePaths:TemplatePath"];

            // Преобразуем в полный путь
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

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

                // Сбрасываем предыдущие результаты перед началом нового подсчета
                counter.Reset();

                _backgroundWorker.ReportProgress(0, "Counting parts...");

                Invoke((MethodInvoker)delegate
                {
                    _selectedDepth = comboBox_ScanDepth.Text; // Получаем текст из ComboBox в потоке UI
                });

                counter.CountParts(oProductDoc.Product, 0, _selectedDepth);
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

            _backgroundWorker.ReportProgress(100, "Saving BOM entries...");
            SaveBomEntry();

            _excelHelper.OpenFile(_saveFileName);
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progress = e.ProgressPercentage;
            string statusMessage = e.UserState as string;

            lbl_Status.Text = $"{statusMessage} ({progress}%)";
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbl_Status.Text = "Done!";
        }

        private void SaveFormData()
        {
            using (StreamWriter writer = new StreamWriter(saveFilePath))
            {
                writer.WriteLine(textBox_Customer.Text);
                writer.WriteLine(textBox_Oem.Text);
                writer.WriteLine(textBox_Project.Text);
            }
        }

        private void LoadFormData()
        {
            if (System.IO.File.Exists(saveFilePath))
            {
                using (StreamReader reader = new StreamReader(saveFilePath))
                {
                    textBox_Customer.Text = reader.ReadLine();
                    textBox_Oem.Text = reader.ReadLine();
                    textBox_Project.Text = reader.ReadLine();
                }
            }
            comboBox_ScanDepth.SelectedIndex = 5;
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            _productionParts.Clear();

            //if (!radioButton_Bg.Checked && !radioButton_Zb.Checked)
            //{
            //    MessageBox.Show("Please slect the unit type");
            //    return;
            //}
            _oRootDocument = _catia.ActiveDocument;
            _backgroundWorker.RunWorkerAsync(); // Запуск работы в фоновом режиме
        }

        private void GetHeadParameters()
        {
            if (_oRootDocument is ProductDocument oProductDoc)
            {
                _savePath = oProductDoc.Path;
                _rootPartNumber = oProductDoc.Product.get_Name();
            }

            SingleNode singleNode = new(_oRootDocument);

            // Используем Invoke для обновления UI из фонового потока
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
            if (_oRootDocument is ProductDocument oProductDoc)
            {
                SingleNode singleNode = new SingleNode(oProductDoc);

                _workSheet = _workBook.GetSheet("bom");

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

                    string formattedDate = _dateTime.ToString("dd.MM.yyyy");
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
            CatiaHelper catiaHelper = new CatiaHelper();

            catiaHelper.CatHelperReset();

            if (_oRootDocument is ProductDocument oProductDoc)
            {
                Product oRootProd = oProductDoc.Product;
                _bomEntry = new BomEntry(_workSheet);

                // Обращение к ComboBox через Invoke
                Invoke((MethodInvoker)delegate
                {
                    _selectedDepth = comboBox_ScanDepth.Text; // Получаем текст из ComboBox в потоке UI
                });

                // Walk down the tree and fill the BOM
                catiaHelper.WalkDownTree(oRootProd, 0 , _selectedDepth);

                // Order by PosNumber
                var orderedNodes = from s in catiaHelper.Nodes
                                   orderby s.PosNumber ascending
                                   select s;
                catiaHelper.Nodes = orderedNodes.ToList();

                int totalNodes = catiaHelper.Nodes.Count;
                int i = 0;

                // Обрабатываем каждый узел и обновляем прогресс
                foreach (var node in catiaHelper.Nodes)
                {
                    _bomEntry.FillBomValues(node, i);
                    i++;

                    // Сообщаем прогресс по мере обработки каждого узла
                    int progress = 40 + (i * 50 / totalNodes); // Прогресс от 40% до 90%
                    _backgroundWorker.ReportProgress(progress);
                }
            }
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveBomEntry()
        {
            //string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            //string testFolderPath = Path.Combine(rootPath, "Test");
            //if (!Directory.Exists(testFolderPath))
            //{
            //    Directory.CreateDirectory(testFolderPath);
            //}
            _saveFileName = _savePath + "\\" + _rootPartNumber + ".xlsx";
            SaveExcelDocument(_saveFileName);
        }

        private static string GetFileOwner(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                var fileSecurity = fileInfo.GetAccessControl();
                var identity = fileSecurity.GetOwner(typeof(System.Security.Principal.NTAccount));
                return identity.ToString();
            }
            catch (Exception)
            {
                return "Unknown user";
            }
        }

        private void SaveExcelDocument(string savePath)
        {
            bool isSaved = false; // Флаг для проверки успешного сохранения

            while (!isSaved)
            {
                try
                {
                    using (var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                    {
                        _workBook.Write(fs);
                        isSaved = true; // Устанавливаем флаг, если сохранение прошло успешно
                    }
                }
                catch (IOException)
                {
                    // Определение, кто использует файл
                    string owner = GetFileOwner(savePath);
                    DialogResult result = MessageBox.Show($"The file is currently opened by {owner}. Please close the file and try again.", "File in use", MessageBoxButtons.OKCancel);

                    if (result == DialogResult.Cancel)
                    {
                        // Прекращаем попытки сохранения, если пользователь выбрал "Отмена"
                        break;
                    }
                }
                catch (Exception ex)
                {
                    // Обработка других исключений
                    MessageBox.Show($"An error occurred: {ex.Message}");
                    break; // Выходим из цикла при возникновении других ошибок
                }
            }

            if (isSaved)
            {
                MessageBox.Show("File saved successfully.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetHeadParameters();
            LoadFormData();
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

        private void ComboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Меняем текст на лейблах в зависимости от выбранного языка
            switch (comboBox_Language.SelectedItem.ToString())
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
