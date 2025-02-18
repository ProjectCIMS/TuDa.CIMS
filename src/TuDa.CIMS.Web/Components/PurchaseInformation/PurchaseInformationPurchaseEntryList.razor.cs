using System.Globalization;
using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationPurchaseEntryList
{
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [Parameter]
    public Guid PurchaseId { get; set; }

   // private readonly IPurchaseApi _purchaseApi = null!;

    // protected override async Task OnInitializedAsync()
    // {
    //     var purchase = await _purchaseApi.GetAsync(WorkingGroupId, PurchaseId);
    //     purchase.Switch(
    //         val => Purchase = val,
    //         err =>
    //             throw new InvalidOperationException(
    //                 $"Could not retrieve Purchase {PurchaseId} of WorkingGroup {WorkingGroupId}. Reason: {string.Join(",", err.Select(e => e.Description))}"
    //             )
    //     );
    // }

    // public PurchaseInformationPurchaseEntryList(IPurchaseApi purchaseApi)
    // {
    //     _purchaseApi = purchaseApi;
    // }

    private Purchase Purchase { get; set; } = null!;
    private List<PurchaseEntry> _purchaseEntries =
    [
        new()
        {
            AssetItem = new Chemical
            {
                Name = "Chemikalie", Room = null!, Cas = null!, PriceUnit = MeasurementUnits.MilliLiter
            },
            Amount = 6,
            PricePerItem = 7
        },

        new()
        {
            AssetItem = new Consumable
            {
                Manufacturer = null!, SerialNumber = null!, Name = "Laborgerät", Room = null!
            },
            Amount = 2,
            PricePerItem = 1.99
        },

        new()
        {
            AssetItem = new Solvent
            {
                Name = "Lösungsmittel", Room = null!, Cas = null!, PriceUnit = MeasurementUnits.MilliLiter
            },
            Amount = 8,
            PricePerItem = 10.99
        },

        new()
        {
            AssetItem = new GasCylinder
            {
                Name = "Technisches Gas", Room = null!, Cas = null!, PriceUnit = MeasurementUnits.MilliLiter
            },
            Amount = 100,
            PricePerItem = 3.99
        }
    ];

    private string GetPricePerItemString(PurchaseEntry purchaseEntry)
    {
        string temp = purchaseEntry.AssetItem switch
        {
            Solvent solvent => solvent.PriceUnit.ToString(),
            Chemical chemical => chemical.PriceUnit.ToString(),
            GasCylinder gasCylinder => gasCylinder.PriceUnit.ToString(),
            _ => "Stück"
        };
        return "Stückpreis: " + $"{purchaseEntry.PricePerItem.ToString(
            "0.00",
            CultureInfo.GetCultureInfo("de-DE")
        )}" + "€" + "/" + temp;
    }

    private string GetTotalPriceString(PurchaseEntry purchaseEntry)
    {
        return "Endpreis: " + purchaseEntry.TotalPrice.ToString("0.00",
            CultureInfo.GetCultureInfo("de-DE")
        ) + "€";
    }

    private string GetAmountString(PurchaseEntry purchaseEntry)
    {
        string temp = purchaseEntry.AssetItem switch
        {
            Solvent solvent => solvent.PriceUnit.ToString(),
            Chemical chemical => chemical.PriceUnit.ToString(),
            GasCylinder gasCylinder => gasCylinder.PriceUnit.ToString(),
            _ => "Stück"
        };
        return "Menge: " + $"{purchaseEntry.Amount }" + " " + temp;
    }

}
