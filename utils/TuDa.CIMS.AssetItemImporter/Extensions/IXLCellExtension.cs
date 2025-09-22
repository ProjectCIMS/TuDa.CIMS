using ClosedXML.Excel;

namespace TuDa.CIMS.AssetItemImporter.Extensions;

/// <summary>
/// Convenience extensions for ClosedXML <see cref="ClosedXML.Excel.IXLCell"/> to
/// retrieve typed values with defaults.
/// </summary>
public static class IXLCellExtension
{
    /// <summary>Get a <see cref="double"/> or 0 if the cell is empty/invalid.</summary>
    public static double GetDoubleOrDefault(this IXLCell cell) =>
        cell.TryGetValue(out double value) ? value : 0;

    /// <summary>Get an <see cref="int"/> or 0 if the cell is empty/invalid.</summary>
    public static int GetIntOrDefault(this IXLCell cell) =>
        cell.TryGetValue(out int value) ? value : 0;
}
