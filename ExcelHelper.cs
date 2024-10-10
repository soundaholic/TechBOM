using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // Для работы с файлами .xlsx
using NPOI.HSSF.UserModel; // Для работы с файлами .xls (Excel 97-2003)
using System;
using System.IO;
using System.Diagnostics;

public class ExcelHelper
{
    public IWorkbook OpenExcelFile(string filePath)
    {
        IWorkbook workbook;

        // Открываем файл Excel
        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            // Определяем, какой тип файла загружен - .xlsx или .xls
            if (Path.GetExtension(filePath).ToLower() == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs); // Файлы Excel 2007 и новее
            }
            else if (Path.GetExtension(filePath).ToLower() == ".xls")
            {
                workbook = new HSSFWorkbook(fs); // Старые файлы Excel (до 2007 года)
            }
            else
            {
                throw new ArgumentException("Формат файла не поддерживается.");
            }
        }

        return workbook;
    }

    public void ReadExcelFile(string filePath)
    {
        IWorkbook workbook = OpenExcelFile(filePath);

        // Предположим, что мы работаем с первым листом
        ISheet sheet = workbook.GetSheetAt(0);

        // Чтение данных
        for (int row = 0; row <= sheet.LastRowNum; row++)
        {
            IRow currentRow = sheet.GetRow(row);

            if (currentRow != null)
            {
                for (int col = 0; col < currentRow.LastCellNum; col++)
                {
                    ICell cell = currentRow.GetCell(col);
                    string cellValue = cell.ToString(); // Получение значения ячейки
                    Console.WriteLine($"Row {row}, Column {col}: {cellValue}");
                }
            }
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

}
