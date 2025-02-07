using Microsoft.AspNetCore.Components;
using MudBlazor;
using SignaturePad;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class SignDialog
{
    public byte[] Signature { get; set; } = Array.Empty<byte>();

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter]
    public required MudDialogInstance MudDialog { get; set; }

    private void Submit()
    {
        MudDialog.Close();
        Snackbar.Add("Der Vorgang wurde erfolgreich abgeschlossen", Severity.Success);
    }

    private void Cancel()
    {
        MudDialog.Cancel();
        Snackbar.Add("Der Vorgang wurde abgebrochen", Severity.Warning);
    }
}
