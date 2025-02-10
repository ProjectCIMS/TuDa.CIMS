using Microsoft.AspNetCore.Components;
using MudBlazor;
using SignaturePad;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class SignDialog
{
    public byte[] Signature { get; set; } = [];

    [CascadingParameter]
    public required MudDialogInstance MudDialog { get; set; }

    private void Submit()
    {
        MudDialog.Close(Signature);
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
