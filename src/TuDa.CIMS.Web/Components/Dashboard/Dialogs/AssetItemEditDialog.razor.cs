using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.Dashboard.Dialogs;

public partial class AssetItemEditDialog
{
    private AssetItemEditForm _assetItemEditForm = null!;
    private readonly ISnackbar _snackbar;
    private readonly IDialogService _dialogService;

    public AssetItemEditDialog(ISnackbar snackbar, IDialogService dialogService)
    {
        _snackbar = snackbar;
        _dialogService = dialogService;
    }

    [CascadingParameter]
    public required MudDialogInstance ProductDialog { get; set; }

    [Parameter]
    public bool ShowError { get; set; }

    [Parameter]
    public required AssetItem Item { get; set; }

    [Parameter]
    public EventCallback<AssetItem> OnDeleteRequested { get; set; }

    /// <summary>
    /// If the Delete was confirmed through the extra dialog this will execute the deletion
    /// </summary>
    private async Task DeleteItem()
    {
        var messageBox = new MessageBoxOptions
        {
            Title = "Eintrag löschen",
            Message = "Wollen sie den Eintrag wirklich löschen?",
            YesText = "Löschen",
            NoText = "Nein",
        };

        if (await _dialogService.ShowMessageBox(messageBox) ?? false)
        {
            await OnDeleteRequested.InvokeAsync(Item);
            ProductDialog.Close();
            _snackbar.Add("Eintrag erfolgreich gelöscht!", Severity.Error);
        }
    }

    /// <summary>
    /// This method will fill the Form with the values of the AssetItem
    /// </summary>
    private void AssetItemEditFormLoad()
    {
        _assetItemEditForm.AssetItemEditFormLoad();
    }

    private void Cancel()
    {
        ProductDialog.Cancel();
    }

    /// <summary>
    /// Method will check for errors and will save the changes if no errors occur
    /// </summary>
    public async Task SaveChanges()
    {
        bool hasErrors = _assetItemEditForm.ErrorsInForm();

        if (hasErrors)
        {
            return;
        }

        await _assetItemEditForm.SaveChanges();
        ProductDialog.Close();
    }
}
