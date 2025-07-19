using ClosedXML.Excel;
using TuDa.CIMS.Shared.Dtos.Create;

namespace TuDa.CIMS.AssetItemImporter.Reader;

public abstract class AssetItemReader
{
    protected readonly XLWorkbook Workbook;
    protected const string Unknown = "Unknown";

    protected AssetItemReader(XLWorkbook workbook) => Workbook = workbook;

    protected AssetItemReader(string path) => Workbook = new XLWorkbook(path);

    public abstract IEnumerable<CreateAssetItemDto> GetAssetItems();
}
