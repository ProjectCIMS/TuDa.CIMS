using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class ShoppingCartSubmitPopupWorkingGroupSelection
{
    /// <summary>
    /// CascadingParameter for working group.
    /// </summary>
    [CascadingParameter]
    public WorkingGroup WorkingGroup { get; set; }

    /// <summary>
    /// Search for the selection of the working group.
    /// </summary>
    private Task<IEnumerable<WorkingGroup>> Search(
        string searchText,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(
            WorkingGroups.Where(w =>
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

    /// <summary>
    /// List of working groups.
    /// </summary>
    [Parameter]
    public required List<WorkingGroup> WorkingGroups { get; set; }
}
