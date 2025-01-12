using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Models;
using TuDa.CIMS.Web.Components.ShoppingCart;

namespace TuDa.CIMS.Web.Components.Dashboard;

public partial class InvoicePage
{
    private static Person s_person1 = new Professor()
    {
        Name = "Herbert",
        Address = new Address(),
        FirstName = "Hans",
        Email = "",
        Gender = Gender.Male,
        Id = Guid.Empty,
        Title = "Prof. Dr.",
        PhoneNumber = "123"
    };

    protected override void OnInitialized()
    {
        SetInvoiceStatistics(Purchase);
    }

    private readonly string _buyer = (Purchase.Buyer is Professor professor ? professor.Title : string.Empty).ToUpper()
                                     + " " + Purchase.Buyer.Name.ToUpper();

    /// <summary>
    /// Selected date range.
    /// </summary>
    private DateRange _dateRange { get; set; } = null!;

    /// <summary>
    /// Parameter Purchase with purchase entries.
    /// </summary>
    [Parameter]
    public static Purchase Purchase { get; set; } = new()
    {
        Buyer = s_person1,
        Id = Guid.Empty,
        CompletionDate = new DateTime(),
        Completed = true,
        Entries = [new PurchaseEntry
            {
                AssetItem = new Chemical
                {
                    Name = null,
                    Room = null,
                    Cas = null,
                    PriceUnit = MeasurementUnits.MilliLiter
                },
                Amount = 5,
                PricePerItem = 10.99
            },
            new PurchaseEntry
            {
                AssetItem = new Consumable
                {
                    Name = null,
                    Room = null,

                    Manufacturer = null,
                    SerialNumber = null
                },
                Amount = 2,
                PricePerItem = 8.99
            },
            new PurchaseEntry
            {
                AssetItem = new Solvent
                {
                    Name = null,
                    Room = null,
                    Cas = null,
                    PriceUnit = MeasurementUnits.MilliLiter
                },
                Amount = 5,
                PricePerItem = 7
            },
            new PurchaseEntry
            {
                AssetItem = new GasCylinder
                {
                    Name = null,
                    Room = null,
                    Cas = null,
                    PriceUnit = MeasurementUnits.MilliLiter
                },
                Amount = 1,
                PricePerItem = 100
            },
        ],
        Signature = []
    };


    private InvoiceStatistics InvoiceStatistics { get; set; } = new()
    {
        TotalPriceConsumables = 0, TotalPriceChemicals = 0, TotalPriceSolvents = 0, TotalPriceGasCylinders = 0
    };

    private void SetInvoiceStatistics(Purchase purchase)
    {
        foreach (var purchaseEntry in purchase.Entries)
        {
            switch (purchaseEntry.AssetItem)
            {
                case Solvent:
                    InvoiceStatistics.TotalPriceSolvents += purchaseEntry.TotalPrice;
                    break;
                case Chemical:
                    InvoiceStatistics.TotalPriceChemicals += purchaseEntry.TotalPrice;
                    break;
                case Consumable:
                    InvoiceStatistics.TotalPriceConsumables += purchaseEntry.TotalPrice;
                    break;
                case GasCylinder:
                    InvoiceStatistics.TotalPriceGasCylinders += purchaseEntry.TotalPrice;
                    break;
            }
        }
    }


    private string GetTotalPriceSolvents()
    {
        return InvoiceStatistics.TotalPriceSolvents.ToString("0.00") + "\u20ac";
    }
    private string GetTotalPriceChemicals()
    {
        return InvoiceStatistics.TotalPriceChemicals.ToString("0.00") + "\u20ac";
    }
    private string GetTotalPriceConsumables()
    {
        return InvoiceStatistics.TotalPriceConsumables.ToString("0.00") + "\u20ac";
    }
    private string GetTotalPriceGasCylinders()
    {
        return InvoiceStatistics.TotalPriceGasCylinders.ToString("0.00") + "\u20ac";
    }
}
