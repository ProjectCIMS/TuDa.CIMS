using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Components.Dashboard.Dialogs;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Dashboard;

public partial class AssetList
{
    /// <summary>
    /// Event callback triggered when the add button is pressed.
    /// </summary>
    [Parameter]
    public EventCallback AddButtonPressed { get; set; }

    /// <summary>
    /// Event callback triggered when the edit button is pressed with the selected AssetItem.
    /// </summary>
    [Parameter]
    public EventCallback<AssetItem> EditButtonPressed { get; set; }

    private readonly IAssetItemApi _assetItemApi;
    private static MudDataGrid<AssetItem> _dataGrid { get; set; }
    private string _searchString { get; set; }

    public AssetList(IAssetItemApi assetItemApi)
    {
        _assetItemApi = assetItemApi;
    }

    /// <summary>
    /// method to reload the datagrid
    /// </summary>
    public async Task ReloadData()
    {
        await _dataGrid.ReloadServerData();
    }

    private async Task<GridData<AssetItem>> ServerReload(GridState<AssetItem> state)
    {
        var errorOrItems = await _assetItemApi.GetAllAsync(_searchString, _selectedTypes);
        if (errorOrItems.IsError)
            return new GridData<AssetItem>();

        var items = SortAssetItems(state, errorOrItems.Value).ToList();

        var filterOptions = new FilterOptions
        {
            FilterCaseSensitivity = DataGridFilterCaseSensitivity.CaseInsensitive,
        };

        foreach (var filterDefinition in state.FilterDefinitions)
        {
            var filterFunction = filterDefinition.GenerateFilterFunction(filterOptions);
            items = items.Where(filterFunction).ToList();
        }

        var pagedData = items.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();

        return new GridData<AssetItem> { TotalItems = items.Count, Items = pagedData };
    }

    private Task OnSearch(string query)
    {
        _searchString = query;
        return _dataGrid.ReloadServerData();
    }

    private IEnumerable<AssetItem> SortAssetItems(
        GridState<AssetItem> state,
        IEnumerable<AssetItem> assetItems
    )
    {
        var sortDefinition = state.SortDefinitions.FirstOrDefault();
        return sortDefinition is null
            ? assetItems
            : assetItems.OrderByDirection(
                GetSortDirection(sortDefinition.Descending),
                sortDefinition.SortFunc
            );
    }

    private static SortDirection GetSortDirection(bool descending) =>
        descending ? MudBlazor.SortDirection.Descending : MudBlazor.SortDirection.Ascending;

    private List<AssetItemType> _selectedTypes = new List<AssetItemType>();

    private async void SelectedChanged(bool value, AssetItemType itemType)
    {
        if (value)
        {
            if (!_selectedTypes.Contains(itemType))
                _selectedTypes.Add(itemType);
        }
        else
        {
            _selectedTypes.Remove(itemType);
        }

        await _dataGrid.ReloadServerData();
    }

    string _icon = Icons.Material.Outlined.FilterAlt;
    bool _filterOpen = false;

    void OpenFilter()
    {
        _filterOpen = true;
    }
}
