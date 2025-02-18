using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationPaper
{
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [Parameter]
    public Guid PurchaseId { get; set; }

    private static Purchase Purchase { get; set; } = new() { Buyer = new Professor
        {
            Name = "Hofmann"
        }
    };
}
