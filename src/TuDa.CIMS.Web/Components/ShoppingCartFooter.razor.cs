using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components;

public partial class ShoppingCartFooter(IDialogService dialogService) : ComponentBase
{
    [Parameter] public Purchase? Purchase { get; set; }

    public double TotalPrice => Purchase?.TotalPrice ?? 0.0;

    private readonly IDialogService _dialogService = dialogService;

    //TODO: Implement the ConfirmAsync method
    private Task ConfirmAsync()
    {
        return null;
    }
}
