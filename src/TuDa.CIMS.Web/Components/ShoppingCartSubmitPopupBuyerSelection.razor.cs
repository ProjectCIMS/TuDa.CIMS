using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components;

public partial class ShoppingCartSubmitPopupBuyerSelection : ComponentBase
{
    /// <summary>
    /// CascadingParameter for working group.
    /// </summary>
    [Parameter]
    public WorkingGroup? WorkingGroup { get; set; }

    [Parameter]
    public required Person Buyer { get; set; }

    /// <summary>
    /// Search for the selection of the student.
    /// </summary>
    private async Task<IEnumerable<Person>> Search(string searchText, CancellationToken cancellationToken)
    {
        if (WorkingGroup == null)
        {
            return [];
        }

        var result = new List<Person>();
        result.AddRange(
            WorkingGroup.Students.Where(s =>
                string.IsNullOrWhiteSpace(searchText)
                || s.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            )
        );
        if (WorkingGroup.Professor.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
        {
            result.Add(WorkingGroup.Professor);
        }
        return result;
    }

    /// <summary>
    /// Returns the name of a given student.
    /// </summary>
    private static string ToString(Person buyer) =>
        buyer switch
        {
            null => "",
            _ => $"{buyer.Name} {buyer.FirstName}",
        };
}
