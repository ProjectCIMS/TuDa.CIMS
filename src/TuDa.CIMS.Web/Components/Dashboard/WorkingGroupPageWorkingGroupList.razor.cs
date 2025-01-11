using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Dashboard;

public partial class WorkingGroupPageWorkingGroupList(IDialogService dialogService)
{
     private MudDataGrid<WorkingGroup> _dataGrid { get; set; }

    private Task OnSearch(string query)
    {
        _searchString = query;
        return _dataGrid.ReloadServerData();
    }
    private readonly IWorkingGroupApi _workingGroupApi;
    private string _searchString { get; set; }

    /// <summary>
    /// Parameter for the create group event.
    /// </summary>
    [Parameter]
    public EventCallback CreateGroupSelected { get; set; }

    private async Task<GridData<WorkingGroup>> ServerReload(GridState<WorkingGroup> state)
    {
        // _workingGroupApi.GetAllAsync(_searchString)
        var errorOrGroups = await _workingGroupApi.GetAllAsync();
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

    private static SortDirection GetSortDirection(bool descending) =>
        descending ? MudBlazor.SortDirection.Descending : MudBlazor.SortDirection.Ascending;

}
