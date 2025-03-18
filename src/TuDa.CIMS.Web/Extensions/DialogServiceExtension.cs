using MudBlazor;
using TuDa.CIMS.Web.Components.WorkingGroupPage;

namespace TuDa.CIMS.Web.Extensions;

public static class DialogServiceExtension
{
    public static async Task<List<string>?> OpenGenericInputPopupAsync(
        this IDialogService dialogService,
        string title,
        List<GenericInputField> fields,
        string? yesText = null
    )
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };

        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { popup => popup.Fields, fields },
        };

        if (yesText is not null)
        {
            parameters.Add(popup => popup.YesText, yesText);
        }

        var dialog = await dialogService.ShowAsync<GenericInputPopUp>(title, parameters, options);

        return await dialog.GetReturnValueAsync<List<string>>();
    }
}
