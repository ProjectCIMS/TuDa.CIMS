using MudBlazor;

namespace TuDa.CIMS.Web.Components;

public partial class ShoppingCartSubmitButton(IDialogService dialogService)
{
    /// <summary>
    /// Opens the popup.
    /// </summary>
    private Task OpenDialogAsync(DialogOptions options)
    {
        return dialogService.ShowAsync<ShoppingCartSubmitPopup>("Custom Options Dialog", options);
    }
}
