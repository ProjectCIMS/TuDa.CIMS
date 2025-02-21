using System.Text;
using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationBuyerInformation
{
    [CascadingParameter]
    public required Purchase Purchase { get; set; }
    private string SignatureAsBase64 => Encoding.UTF8.GetString(Purchase.Signature);
}
