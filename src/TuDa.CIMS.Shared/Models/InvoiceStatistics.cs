namespace TuDa.CIMS.Shared.Models;

public record InvoiceStatistics
{
    public required double TotalPriceConsumables { get; set; }
    public required double TotalPriceChemicals { get; set; }
    public required double TotalPriceSolvents { get; set; }
    public required double TotalPriceGasCylinders { get; set; }
}
