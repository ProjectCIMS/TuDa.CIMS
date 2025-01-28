using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupList;

public partial class WorkingGroupPageWorkingGroupList
{
    public WorkingGroupPageWorkingGroupList(IWorkingGroupApi workingGroupApi)
    {
        _workingGroupApi = workingGroupApi;
    }

    private MudDataGrid<WorkingGroup> _dataGrid { get; set; } = new();

    private readonly IWorkingGroupApi _workingGroupApi;

    private string _searchString { get; set; } = string.Empty;

    [Inject] private NavigationManager _navigationManager { get; set; } = null!;

    private void GoToWorkingGroupInfoPage(DataGridRowClickEventArgs<WorkingGroup> args)
    {
        var id = args.Item.Id;
        _navigationManager.NavigateTo($"working-groups/{id}");
    }

    /// <summary>
    /// Parameter for the create group event.
    /// </summary>
    [Parameter]
    public EventCallback CreateGroupSelected { get; set; }

    private async Task<GridData<WorkingGroup>> ServerReload(GridState<WorkingGroup> state)
    {
        var errorOrGroups = await _workingGroupApi.GetAllAsync(_searchString);
        if (errorOrGroups.IsError)
            return new GridData<WorkingGroup>();
        var groups = SortWorkingGroups(state, errorOrGroups.Value).ToList();
        var pagedData = groups.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();
        return new GridData<WorkingGroup> { TotalItems = groups.Count, Items = pagedData };
    }

    private IEnumerable<WorkingGroup> SortWorkingGroups(
        GridState<WorkingGroup> state,
        IEnumerable<WorkingGroup> workingGroups
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
