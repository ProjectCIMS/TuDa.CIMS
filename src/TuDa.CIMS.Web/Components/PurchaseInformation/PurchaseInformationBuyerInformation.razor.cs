using System.Text;
using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.PurchaseInformation;

public partial class PurchaseInformationBuyerInformation
{
    [Parameter]
    public required PurchaseResponseDto Purchase { get; set; }
    // TODO: Dto for the signature is needed here
    //private string SignatureAsBase64 => Encoding.UTF8.GetString(Purchase.Signature);
}
