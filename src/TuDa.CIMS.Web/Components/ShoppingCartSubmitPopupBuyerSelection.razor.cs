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
    public  WorkingGroup? WorkingGroup { get; set; }

    [Parameter]
    public required Person Buyer { get; set; }

    /// <summary>
    /// Search for the selection of the student.
    /// </summary>
    private Task<IEnumerable<Person>> Search(
        string searchText,
        CancellationToken cancellationToken
    )
    {
        if (WorkingGroup != null)
        {
            var result = WorkingGroup.Students
                .Where(s => (EF.Functions.Like(s.Name.ToLower(), $"{searchText.ToLower()}%")) || (EF.Functions.Like(s.FirstName.ToLower(), $"{searchText.ToLower()}%")) )
                .Cast<Person>();
            return Task.FromResult(result);
        }
        return Task.FromResult(Enumerable.Empty<Person>());
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

