using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components;

public partial class ProductDialog
{
    [CascadingParameter] private MudDialogInstance _productDialog { get; set; }

    // Product to be shown in Dialog.
    [Parameter] public AssetItem Product { get; set; }

    // Amount of Product
    [Parameter] public int Amount { get; set; }

    // Simple Functions to Submit and Cancel the Action.
    private void Submit() => _productDialog.Close(DialogResult.Ok(Amount));
    private void Cancel() => _productDialog.Cancel();


    /// <summary>
    /// Method to add Product to Product List.
    /// </summary>
    private void AddProduct()
    {
        // Logic to add the product goes here.
        // For now, just close the dialog after clicking "Add".
        if (Amount > 0)
        {
            Console.WriteLine($"Product added with quantity: {Amount}");
            Submit();
        }
        else
        {
            Console.WriteLine("Quantity must be greater than 0");
        }
    }
}
