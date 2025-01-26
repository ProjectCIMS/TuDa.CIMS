using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.Dashboard.Dialogs;

public partial class AssetItemEditDialog
{
    private AssetItemEditForm _assetItemEditForm;

    [CascadingParameter]
    public required MudDialogInstance ProductDialog { get; set; }

    [Inject]
    public required IDialogService DialogService { get; set; }

    [Parameter]
    public bool ShowError { get; set; }

    [Parameter]
    public required AssetItem Item { get; set; }

    /// <summary>
    /// Opening up the Delete Confirmation Dialog
    /// </summary>
    private Task DeleteAsync()
    {
        var parameters = new DialogParameters<ConfirmDeleteDialog>
        {
            {
                x => x.ContentText,
                "Willst du diesen Gegenstand wirklich löschen? Dies kann nicht rückgängig gemacht werden"
            },
            { x => x.ButtonText, "Löschen" },
            { x => x.Color, Color.Error },
            { "OnDeleteRequested", EventCallback.Factory.Create(this, DeleteItem) },
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        return DialogService.ShowAsync<ConfirmDeleteDialog>(
            "Gegenstand löschen",
            parameters,
            options
        );
    }

    /// <summary>
    /// If the Delete was confirmed through the extra dialog this will execute the deletion
    /// </summary>
    private async Task DeleteItem()
    {
        await _assetItemEditForm.DeleteItem();
        ProductDialog.Close();
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
        _assetItemEditForm.ShowError = false;
        ProductDialog.Cancel();
    }

    /// <summary>
    /// Method will check for errors and will save the changes if no errors occur
    /// </summary>
    public async Task SaveChanges()
    {
        bool hasErrors = _assetItemEditForm.ValidateForm();

        if (hasErrors)
        {
            return;
        }

        await _assetItemEditForm.SaveChanges();
        ProductDialog.Close();
    }
}
