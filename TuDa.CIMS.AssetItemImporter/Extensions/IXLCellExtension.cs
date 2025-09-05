using ClosedXML.Excel;

namespace TuDa.CIMS.AssetItemImporter.Extensions;

public static class IXLCellExtension
{
    public static double TryGetDouble(this IXLCell cell) =>
        cell.TryGetValue(out double value) ? value : 0;

    public static int TryGetInt(this IXLCell cell) => cell.TryGetValue(out int value) ? value : 0;
}
