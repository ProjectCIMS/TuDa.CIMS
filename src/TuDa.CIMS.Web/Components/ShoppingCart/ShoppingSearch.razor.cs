using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

/// <summary>
///Represents a component that allows users to search for and select an <see cref="AssetItem"/> on the ShoppingCart Page.
/// </summary>
public partial class ShoppingSearch : ComponentBase
{
    private readonly IAssetItemApi _assetItemApi;
    private MudAutocomplete<AssetItem> _autocomplete = null!; // Is set by blazor component

    /// <summary>
    /// Event that is called when an <see cref="AssetItem"/> is selected.
    /// </summary>
    [Parameter]
    public EventCallback<AssetItem> AssetItemSelected { get; set; }

    public ShoppingSearch(IAssetItemApi api)
    {
        _assetItemApi = api;
    }

    /// Invoke to clear text
    private async Task AssetItemSelectedInternal(AssetItem item)
    {
        await _autocomplete.ResetAsync();
        await AssetItemSelected.InvokeAsync(item);
    }

    /// <summary>
    /// To filter for different types of Items
    /// </summary>
    private AssetItemType? _selectedAssetItemType;

    private async Task<IEnumerable<AssetItem>> Search(string nameOrCas, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(nameOrCas))
        {
            return [];
        }

        var allItems = await _assetItemApi
            .GetAllAsync(nameOrCas)
            .Match(value => value, err => new List<AssetItem>());

        return _selectedAssetItemType switch
        {
            AssetItemType.Chemical => allItems.Where(item => item is Chemical and not Solvent),
            AssetItemType.Consumable => allItems.OfType<Consumable>(),
            AssetItemType.Solvent => allItems.OfType<Solvent>(),
            AssetItemType.GasCylinder => allItems.OfType<GasCylinder>(),
            _ => allItems,
        };
    }

    private static string ToString(AssetItem item) =>
        item switch
        {
            Substance substance => $"{substance.Name} ({substance.Cas})",
            null => "",
            _ => item.Name,
        };
}
