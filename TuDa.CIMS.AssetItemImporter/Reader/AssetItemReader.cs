using ClosedXML.Excel;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.AssetItemImporter.Reader;

public abstract class AssetItemReader
{
    protected readonly XLWorkbook Workbook;
    protected const string Unknown = "Unknown";

    protected AssetItemReader(XLWorkbook workbook) => Workbook = workbook;

    protected AssetItemReader(string path) => Workbook = new XLWorkbook(path);

    public abstract IEnumerable<CreateAssetItemDto> GetAssetItems();

    protected static Rooms GetRoom(string roomString) =>
        Enum.TryParse(roomString, out Rooms room) ? room : Rooms.None;
}
