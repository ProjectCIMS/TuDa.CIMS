using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components;

/// <summary>
///Represents a component that allows users to search for and select an <see cref="AssetItem"/> on the ShoppingCart Page.
/// </summary>
public partial class ShoppingSearch : ComponentBase
{
    private readonly IAssetItemApi _assetItemApi;

    /// <summary>
    /// Event that is called when an <see cref="AssetItem"/> is selected.
    /// </summary>
    [Parameter]
    public EventCallback<AssetItem> AssetItemSelected { get; set; }

    public ShoppingSearch(IAssetItemApi api)
    {
        _assetItemApi = api;
    }

    private async Task<IEnumerable<AssetItem>> Search(string nameOrCas, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(nameOrCas))
        {
            return [];
        }

        return await _assetItemApi.GetAllAsync(nameOrCas).Match(value => value, err => []);
    }

    private static string ToString(AssetItem item) =>
        item switch
        {
            Substance substance => $"{substance.Name} ({substance.Cas})",
            _ => item.Name,
        };
}
