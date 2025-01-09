using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.Dashboard;

public partial class WorkingGroupList(IDialogService dialogService)
{
    /// <summary>
    /// List of available WorkingGroups.
    /// </summary>

    [Parameter]
    public required List<WorkingGroup> WorkingGroups { get; set; }


    /// <summary>
    /// quick filter for filtering globally across multiple columns with the same input
    /// </summary>
    private Func<WorkingGroup, bool> QuickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if (x.Professor.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.PhoneNumber.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    // private MudDataGrid<WorkingGroup> _dataGrid { get; set; }

    // private Task OnSearch(string query)
    // {
    //     _searchString = query;
    //     return _dataGrid.ReloadServerData();
    // }
    //   private readonly IWorkingGroupApi _workingGroupApi;
    private string _searchString { get; set; }

    /// <summary>
    /// Parameter for the create group event.
    /// </summary>
    [Parameter]
    public EventCallback CreateGroupSelected { get; set; }

    /// <summary>
    /// Opens the popup.
    /// </summary>
    private Task OpenDialogAsync(DialogOptions options)
    {
        return dialogService.ShowAsync<WorkingGroupListAddDialog>("Custom Options Dialog", options);
    }


    // private async Task<GridData<WorkingGroup>> ServerReload(GridState<WorkingGroup> state)
    // {
    //     var errorOrItems = await _workingGroupApi.GetAllAsync(_searchString);
    //     if (errorOrItems.IsError)
    //         return new GridData<WorkingGroup>();
    //     var groups = SortWorkingGroups(state, errorOrItems.Value).ToList();
    //     var pagedData = groups.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();
    //     return new GridData<WorkingGroup> { TotalItems = groups.Count, Items = pagedData };
    // }

    /*private IEnumerable<WorkingGroup> SortWorkingGroups(
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
    }*/

    // private static SortDirection GetSortDirection(bool descending) =>
    //     descending ? MudBlazor.SortDirection.Descending : MudBlazor.SortDirection.Ascending;
    // private Task OpenDialogAsync()
    // {
    //     var options = new DialogOptions { CloseOnEscapeKey = true };
    //
    //     return DialogService.ShowAsync<WorkingGroupListAddDialog>("Simple Dialog", options);
    // }
}
