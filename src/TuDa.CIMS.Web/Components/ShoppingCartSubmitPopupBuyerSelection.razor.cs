using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components;

public partial class ShoppingCartSubmitPopupBuyerSelection : ComponentBase
{
    /// <summary>
    /// CascadingParameter for working group.
    /// </summary>
    [Parameter]
    public required WorkingGroup WorkingGroup { get; set; }

    [Parameter]
    public required Student Student { get; set; }

    /// <summary>
    /// Search for the selection of the student.
    /// </summary>
    private Task<IEnumerable<Student>> Search(
        string searchText,
        CancellationToken cancellationToken
    )
    {
        return Task.FromResult(
            WorkingGroup.Students.Where(s =>
                string.IsNullOrWhiteSpace(searchText)
                || s.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            )
        );
    }

    /// <summary>
    /// Returns the name of a given student.
    /// </summary>
    private static string ToString(Student student) =>
        student switch
        {
            null => "",
            _ => student.Name,
        };
}

