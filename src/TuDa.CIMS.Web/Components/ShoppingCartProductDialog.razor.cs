using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components;

public partial class ShoppingCartProductDialog
{
    [CascadingParameter]
    public required MudDialogInstance ProductDialog { get; set; }

    [Inject]
    private ILogger<ShoppingCartProductDialog> Logger { get; set; }

    // Product to be shown in Dialog.
    [Parameter]
    public required AssetItem Product { get; set; }

    // Amount of Product
    public int Amount { get; set; } = 1;
    private bool IsError => Amount <= 0;

    // Simple Functions to Submit and Cancel the Action.
    private void Submit() => ProductDialog.Close(DialogResult.Ok(Amount));

    private void Cancel() => ProductDialog.Cancel();

    /// <summary>
    /// Check if Amount is higher than 0.
    /// false: dialog will not be closed.
    /// true: submit will be successful.
    /// </summary>
    private void CheckAndSubmit()
    {
        if (Amount > 0)
        {
            Logger.LogInformation($"Product added with quantity: {Amount}");
            Submit();
        }
        else
        {
            Logger.LogWarning("Quantity must be greater than 0");
        }
    }
}
