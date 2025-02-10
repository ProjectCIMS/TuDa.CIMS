using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Constants;
using TuDa.CIMS.Shared.Entities.Enums;
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
    private MudDataGrid<AssetItem> _dataGrid { get; set; }
    private string _searchString { get; set; }

    private List<AssetItemType>? _selectedTypes { get; set; }

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
        var filterDto = new AssetItemFilterDto
        {
            NameOrCas = _searchString,
            AssetItemTypes = _selectedTypes,
        };

        foreach (var filter in state.FilterDefinitions)
        {
            switch (filter.Column.Title)
            {
                case AssetItemColumns.Product:
                    filterDto.AssetItemColumnsList.Add(AssetItemColumns.Product);
                    filterDto.Product = filter.Value.ToString();
                    break;
                case AssetItemColumns.Shop:
                    filterDto.AssetItemColumnsList.Add(AssetItemColumns.Shop);
                    filterDto.Shop = filter.Value.ToString();
                    break;
                case AssetItemColumns.ItemNumber:
                    filterDto.AssetItemColumnsList.Add(AssetItemColumns.ItemNumber);
                    filterDto.ItemNumber = filter.Value.ToString();
                    break;
                case AssetItemColumns.RoomName:
                    filterDto.AssetItemColumnsList.Add(AssetItemColumns.RoomName);
                    filterDto.RoomName = filter.Value.ToString();
                    break;
                case AssetItemColumns.Price:
                    filterDto.AssetItemColumnsList.Add(AssetItemColumns.Price);
                    filterDto.Price = filter.Value.ToString();
                    break;
            }
        }

        var errorOrItems = await _assetItemApi.GetAllAsync(filterDto);
        if (errorOrItems.IsError)
            return new GridData<AssetItem>();

        var items = SortAssetItems(state, errorOrItems.Value).ToList();

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

    private async Task SelectedChanged(bool value, AssetItemType itemType)
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

    void ToggleFilter()
    {
        _selectedTypes = new List<AssetItemType>();
        ReloadData();
        _filterOpen = !_filterOpen;
    }
}
