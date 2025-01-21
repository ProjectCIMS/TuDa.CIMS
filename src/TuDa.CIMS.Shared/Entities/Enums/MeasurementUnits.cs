namespace TuDa.CIMS.Shared.Entities.Enums;

public enum MeasurementUnits
{
    Unitless,
    MilliLiter,
    Liter,
    Gram,
    KiloGram,
    Piece,
}

public static class MeasurementUnitsExtension
{
    public static string ToAbbrevation(this MeasurementUnits unit) =>
        unit switch
        {
            MeasurementUnits.KiloGram => "kg",
            MeasurementUnits.Liter => "l",
            MeasurementUnits.Piece => "Stück",
            MeasurementUnits.MilliLiter => "ml",
            MeasurementUnits.Gram => "g",
            MeasurementUnits.Unitless => "",
            _ => $" {unit}",
        };
}
