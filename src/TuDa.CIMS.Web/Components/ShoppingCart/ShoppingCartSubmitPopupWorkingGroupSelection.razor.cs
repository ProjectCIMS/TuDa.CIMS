using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class ShoppingCartSubmitPopupWorkingGroupSelection
{
    [Parameter]
    public required WorkingGroup WorkingGroup { get; set; }

    /// <summary>
    /// List of working groups.
    /// TODO: Need to be replaced by WorkingGroupApi
    /// </summary>
    [Parameter]
    public List<WorkingGroup> ListOfWorkingGroups { get; set; } = [];

    /// <summary>
    /// Search for the selection of the working group.
    /// </summary>
    private Task<IEnumerable<WorkingGroup>> Search(
        string searchText,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(
            ListOfWorkingGroups.Where(w =>
                string.IsNullOrWhiteSpace(searchText)
                || w.Professor.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            )
        );
    }

    /// <summary>
    /// Returns the name of a given working group.
    /// </summary>
    private static string ToString(WorkingGroup workingGroup) =>
        workingGroup switch
        {
            null => "",
            _ => workingGroup.Professor.Name,
        };

}
