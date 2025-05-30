using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupList;

public partial class WorkingGroupPageWorkingGroupList
{
    private readonly IWorkingGroupApi _workingGroupApi;
    private readonly NavigationManager _navigationManager;

    public WorkingGroupPageWorkingGroupList(
        IWorkingGroupApi workingGroupApi,
        NavigationManager navigationManager
    )
    {
        _workingGroupApi = workingGroupApi;
        _navigationManager = navigationManager;
    }

    private MudDataGrid<WorkingGroupResponseDto> _dataGrid { get; set; } = new();

    private string _searchString { get; set; } = string.Empty;

    private void GoToWorkingGroupInfoPage(DataGridRowClickEventArgs<WorkingGroupResponseDto> args)
    {
        var id = args.Item.Id;
        _navigationManager.NavigateTo($"working-groups/{id}");
    }

    /// <summary>
    /// Parameter for the create group event.
    /// </summary>
    [Parameter]
    public EventCallback CreateGroupSelected { get; set; }

    private async Task<GridData<WorkingGroupResponseDto>> ServerReload(
        GridState<WorkingGroupResponseDto> state
    )
    {
        var errorOrGroups = await _workingGroupApi.GetAllAsync(_searchString);
        if (errorOrGroups.IsError)
            return new GridData<WorkingGroupResponseDto>();
        var groups = SortWorkingGroups(state, errorOrGroups.Value).ToList();
        var pagedData = groups.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();
        return new GridData<WorkingGroupResponseDto>
        {
            TotalItems = groups.Count,
            Items = pagedData,
        };
    }

    private IEnumerable<WorkingGroupResponseDto> SortWorkingGroups(
        GridState<WorkingGroupResponseDto> state,
        IEnumerable<WorkingGroupResponseDto> workingGroups
    )
    {
        var sortDefinition = state.SortDefinitions.FirstOrDefault();
        return sortDefinition is null
            ? workingGroups
            : workingGroups.OrderByDirection(
                GetSortDirection(sortDefinition.Descending),
                sortDefinition.SortFunc
            );
    }

    public Task ReloadDataGridAsync() => _dataGrid.ReloadServerData();

    private Task OnSearch(string query)
    {
        _searchString = query;
        return _dataGrid.ReloadServerData();
    }

    private static SortDirection GetSortDirection(bool descending) =>
        descending ? MudBlazor.SortDirection.Descending : MudBlazor.SortDirection.Ascending;
}
