using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;
using MECMOD;
using TechBOM.SingleNodeDomain;

namespace TechBOM
{
    public class ExcelProcessor
    {
        private string _saveFileName;
        private RootNode _rootNode;

        public string TemplateFilePath { get; private set; }
        public IWorkbook WorkBook { get; private set; }
        public ISheet WorkSheet { get; private set; }

        public ExcelProcessor(RootNode rootNode)
        {
            _rootNode = rootNode;
            _saveFileName = rootNode.SaveFileName;
            TemplateFilePath = GetTemplatePath();
            WorkBook = GetWorkBook();
            WorkSheet = GetWorkSheet("bom");
        }

        public void FillExcelHeadData(SingleNode singleNode, ExcelHeadData headData, List<PartDocument> productionParts)
        {
            DateTime dateTime = DateTime.Now;

            ISheet workSheet = WorkBook.GetSheet("bom");

            if (singleNode.IsRoot)
            {
                IRow oRowPartNumber = workSheet.GetRow(5);
                ICell oCellPartNumber = oRowPartNumber.GetCell(7);
                oCellPartNumber.SetCellValue(singleNode.Data.Name);

                IRow oRootOrderNumber = workSheet.GetRow(6);
                ICell oCellRootOrderNumber = oRootOrderNumber.GetCell(7);
                oCellRootOrderNumber.SetCellValue(singleNode.Data.DrawingNumber);

                IRow oRowCustomer = workSheet.GetRow(2);
                ICell oCellCustomer = oRowCustomer.GetCell(3);
                oCellCustomer.SetCellValue(headData.Customer);

                IRow oRowOem = workSheet.GetRow(3);
                ICell oCellOem = oRowOem.GetCell(3);
                oCellOem.SetCellValue(headData.Oem);

                IRow oRowProject = workSheet.GetRow(4);
                ICell oCellProject = oRowProject.GetCell(3);
                oCellProject.SetCellValue(headData.Project);

                string formattedDate = dateTime.ToString("dd.MM.yyyy");
                IRow oRowDate = workSheet.GetRow(7);
                ICell oCellDate = oRowDate.GetCell(3);
                oCellDate.SetCellValue(formattedDate);

                IRow oRowPlant = workSheet.GetRow(2);
                ICell oCellPlant = oRowPlant.GetCell(7);
                oCellPlant.SetCellValue(headData.Plant);

                IRow oRowLine = workSheet.GetRow(3);
                ICell oCellLine = oRowLine.GetCell(7);
                oCellLine.SetCellValue(headData.Line);

                IRow oRowStation = workSheet.GetRow(4);
                ICell oCellStation = oRowStation.GetCell(7);
                oCellStation.SetCellValue(headData.Station);

                IRow oRowRevision = workSheet.GetRow(7);
                ICell oCellRevision = oRowRevision.GetCell(7);
                oCellRevision.SetCellValue(headData.Version);
            }
        }

        public void SaveExcelDocument(string savePath)
        {
            bool isSaved = false;

            while (!isSaved)
            {
                try
                {
                    using var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write);
                    WorkBook.Write(fs);
                    isSaved = true;
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

        public string GetFileOwner(string filePath)
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

        public void OpenFile(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось открыть файл: {ex.Message}");
            }
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

            var filePath = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
            if (!filePath.Exists)
            {
                throw new DirectoryNotFoundException("Template file does not exist");
            }

            return filePath.FullName;
        }

        private IWorkbook GetWorkBook()
        {
            IWorkbook workBook;
            using (FileStream file = new(TemplateFilePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workBook = new XSSFWorkbook(file);
            }
            return workBook;
        }

        private ISheet GetWorkSheet(string sheetName)
        {
            ISheet workSheet = WorkBook.GetSheet(sheetName);
            return workSheet;
        }
    }
}


