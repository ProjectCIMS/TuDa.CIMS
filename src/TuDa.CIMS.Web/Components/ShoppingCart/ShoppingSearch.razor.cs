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
    private List<AssetItemType> _selectedAssetItemType = [];

    private async Task<IEnumerable<AssetItem>> Search(string nameOrCas, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(nameOrCas))
        {
            return [];
        }

        return await _assetItemApi
            .GetAllAsync(nameOrCas, _selectedAssetItemType)
            .Match(value => value, err => []);
    }

    private static string ToString(AssetItem item) =>
        item switch
        {
            Substance substance => $"{substance.Name} ({substance.Cas})",
            null => "",
            _ => item.Name,
        };
}
