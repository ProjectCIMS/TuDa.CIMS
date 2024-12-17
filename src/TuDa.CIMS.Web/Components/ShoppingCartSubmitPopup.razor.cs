using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components;

public partial class ShoppingCartSubmitPopup
{
    /// <summary>
    /// Returns the name of a given working group.
    /// </summary>
    private static string ToString(WorkingGroup workingGroup) =>
        workingGroup switch
        {
            null => "",
            _ => workingGroup.Professor.Name,
        };

    private static readonly Professor s_prof1 = new() { Name = "Prof. Mustermann", };

    private static readonly Professor s_prof2 = new() { Name = "Prof. Schmidt", };

    private static readonly Professor s_prof3 = new() { Name = "Prof. Müller", };

    private static readonly Professor s_prof4 = new() { Name = "Prof. Hofmann", };

    private static readonly Professor s_prof5 = new() { Name = "Prof. Meyer", };

    public WorkingGroup WorkingGroup { get; set; }

    [Parameter]
    public List<WorkingGroup> WorkingGroups { get; set; } =
    [
        new()
        {
            Id = Guid.Empty,
            PhoneNumber = "",
            Professor = s_prof1,
            Purchases = [],
            Students = []
        },
        new()
        {
            Id = Guid.Empty,
            PhoneNumber = "",
            Professor = s_prof2,
            Purchases = [],
            Students = []
        },
        new()
        {
            Id = Guid.Empty,
            PhoneNumber = "",
            Professor = s_prof3,
            Purchases = [],
            Students = []
        },
        new()
        {
            Id = Guid.Empty,
            PhoneNumber = "",
            Professor = s_prof4,
            Purchases = [],
            Students = []
        },
        new()
        {
            Id = Guid.Empty,
            PhoneNumber = "",
            Professor = s_prof5,
            Purchases = [],
            Students = []
        }
    ];

    /// <summary>
    /// Search for the selection of the working group.
    /// </summary>
    private Task<IEnumerable<WorkingGroup>> Search(string searchText, CancellationToken cancellationToken)
    {
        return Task.FromResult(WorkingGroups
            .Where(w =>
                string.IsNullOrWhiteSpace(searchText) ||
                w.Professor.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)));
    }

    /// <summary>
    /// CascadingParamter MudDialag.
    /// </summary>
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }

    /// <summary>
    /// Closes the MudDialog.
    /// </summary>
    private void Submit() => MudDialog.Close(DialogResult.Ok(WorkingGroup));

    /// <summary>
    /// Cancels the MudDialog.
    /// </summary>
    private void Cancel() => MudDialog.Cancel();

    /// <summary>
    /// List of purchase entries to be shown.
    /// </summary>
    [Parameter]
    public static List<PurchaseEntry> PurchaseEntries { get; set; } =
    [
        new()
        {
            Amount = 30,
            AssetItem = new Consumable()
            {
                Amount = 60,
                Id = Guid.Empty,
                ItemNumber = "123456",
                Manufacturer = " Carl Roth GmbH",
                Name = "Erlenmeyerkolben",
                Note = "Glasgefäß",
                Price = 30.99,
                Room = new Room() { Id = Guid.Empty, Name = "Raum-A" },
                SerialNumber = "112233445566",
                Shop = "Chemieladen"
            },
            PricePerItem = 30.99,
            Id = Guid.Empty
        },
        new()
        {
            Amount = 1500,
            AssetItem = new Solvent()
            {
                BindingSize = 3.5,
                Id = Guid.Empty,
                ItemNumber = "123456",
                Cas = "7647-01-0",
                Name = "Salzsäure",
                Note = "Chlorwasserstoff",
                Price = 10.99,
                Room = new Room() { Id = Guid.Empty, Name = "Raum-B" },
                Shop = "Chemieladen",
                Hazards = [],
                Purity = 0.75,
                PriceUnit = PriceUnits.PerLiter
            },
            PricePerItem = 10.99,
            Id = Guid.Empty
        },
        new()
        {
            Amount = 5,
            AssetItem = new Chemical()
            {
                BindingSize = 15.5,
                Id = Guid.Empty,
                ItemNumber = "123456",
                Cas = "124-38-9",
                Name = "Trockeneis",
                Note = "kalt",
                Price = 99.99,
                Room = new Room() { Id = Guid.Empty, Name = "Raum-C" },
                Shop = "Chemieladen",
                Hazards = [],
                Purity = 0.99,
                PriceUnit = PriceUnits.PerKilo
            },
            PricePerItem = 30.99,
            Id = Guid.Empty
        },
        new()
        {
            Amount = 1,
            AssetItem = new Consumable()
            {
                Amount = 100,
                Id = Guid.Empty,
                ItemNumber = "123456",
                Manufacturer = "Glasmacher",
                Name = "Reagenzglas",
                Note = "Glasgefäß",
                Price = 7.99,
                Room = new Room() { Id = Guid.Empty, Name = "Raum-Z" },
                SerialNumber = "01248",
                Shop = "Chemieladen2"
            },
            PricePerItem = 7.99,
            Id = Guid.Empty
        },
        new()
        {
            Amount = 1,
            AssetItem = new Consumable()
            {
                Amount = 100,
                Id = Guid.Empty,
                ItemNumber = "123456",
                Manufacturer = "Röhrchenzentrale",
                Name = "Analysenröhrchen",
                Note = "genug im Lager",
                Price = 17.99,
                Room = new Room() { Id = Guid.Empty, Name = "Raum-120" },
                SerialNumber = "0124816",
                Shop = "Rohr-Shop"
            },
            PricePerItem = 17.99,
            Id = Guid.Empty
        },
        new()
        {
            Amount = 15,
            AssetItem = new Consumable()
            {
                Amount = 100,
                Id = Guid.Empty,
                ItemNumber = "123456",
                Manufacturer = "Glasmacher",
                Name = "Becherglas",
                Note = "Glasgefäß",
                Price = 7.99,
                Room = new Room() { Id = Guid.Empty, Name = "Raum-A5" },
                SerialNumber = "012481012",
                Shop = "Glas-Shop"
            },
            PricePerItem = 7.99,
            Id = Guid.Empty
        },
        new()
        {
            Amount = 6,
            AssetItem = new Consumable()
            {
                Amount = 100,
                Id = Guid.Empty,
                ItemNumber = "12345686775",
                Manufacturer = "Glasmacher",
                Name = "Birnenkolben",
                Note = "keine Birne",
                Price = 10.99,
                Room = new Room() { Id = Guid.Empty, Name = "Raum-B101" },
                SerialNumber = "0124823467",
                Shop = "Shop8"
            },
            PricePerItem = 7.99,
            Id = Guid.Empty
        },
        new()
        {
            Amount = 6,
            AssetItem = new Consumable()
            {
                Amount = 100,
                Id = Guid.Empty,
                ItemNumber = "12345646775",
                Manufacturer = "",
                Name = "Faltenfilter",
                Note = "nützlich",
                Price = 7.99,
                Room = new Room() { Id = Guid.Empty, Name = "Raum-Z" },
                SerialNumber = "01248",
                Shop = "Chemieladen2"
            },
            PricePerItem = 7.99,
            Id = Guid.Empty
        }
    ];

    /// <summary>
    /// Returns the amount with the respective price unit of the given purchase entry as a string.
    /// </summary>
    private static string GetAmountText(PurchaseEntry purchaseEntry) =>
        $"{purchaseEntry.Amount}" + purchaseEntry.AssetItem switch
        {
            Substance substance => substance.PriceUnit switch
            {
                PriceUnits.PerKilo => " kg",
                PriceUnits.PerLiter => " l",
                PriceUnits.PerPiece => " Stück",
                _ => $" {substance.PriceUnit}"
            },
            _ => " Stück"
        };
}
