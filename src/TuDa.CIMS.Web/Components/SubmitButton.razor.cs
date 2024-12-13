using MudBlazor;

namespace TuDa.CIMS.Web.Components;

public partial class SubmitButton
{
    /// <summary>
    /// DialogOption to implement the close button.
    /// </summary>
    private readonly DialogOptions _closeButton = new() {CloseButton = true};

    /// <summary>
    /// Opens the popup.
    /// </summary>

    private Task OpenDialogAsync(DialogOptions options)
    {
        return Dialog.ShowAsync<SubmitPopup>("Custom Options Dialog", options);
    }
}
