using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Dtos.Responses;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationBuyerInformation
{
    [CascadingParameter]
    public required PurchaseResponseDto Purchase { get; set; }
    // TODO: Dto for the signature is needed here
    //private string SignatureAsBase64 => Encoding.UTF8.GetString(Purchase.Signature);
}
