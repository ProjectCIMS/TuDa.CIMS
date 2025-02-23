using System.ComponentModel;

namespace TuDa.CIMS.Shared.Entities.Enums;

public enum MeasurementUnits
{
    MilliLiter,
    Liter,
    Gram,
    KiloGram,
    Piece,
}

public static class MeasurementUnitsExtension
{
    public static string ToAbbreviation(this MeasurementUnits unit) =>
        unit switch
        {
            MeasurementUnits.KiloGram => "kg",
            MeasurementUnits.Liter => "l",
            MeasurementUnits.Piece => "Stück",
            MeasurementUnits.MilliLiter => "ml",
            MeasurementUnits.Gram => "g",
            _ => $" {unit}",
        };

    public static string ToDocumentAbbreviation(this MeasurementUnits unit) =>
        unit switch
        {
            MeasurementUnits.KiloGram => "kg",
            MeasurementUnits.Liter => "l",
            MeasurementUnits.Piece => "Stk.",
            MeasurementUnits.MilliLiter => "ml",
            MeasurementUnits.Gram => "g",
            _ => throw new InvalidEnumArgumentException(),
        };
}
