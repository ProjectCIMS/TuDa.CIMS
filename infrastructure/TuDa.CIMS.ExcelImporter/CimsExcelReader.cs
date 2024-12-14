using System.Reflection;
using ClosedXML.Excel;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.ExcelImporter;

public abstract class CimsExcelReader
{
    protected readonly XLWorkbook Workbook;
    protected readonly Room StubRoom = new Room { Name = "None" };
    protected readonly string StubCas = "Unknown";

    protected CimsExcelReader(string path)
    {
        Workbook = new XLWorkbook(path);
    }

    protected static string ExcelFilePath(string fileName) => $"{Assembly.GetExecutingAssembly().Location}/{fileName}";
}
