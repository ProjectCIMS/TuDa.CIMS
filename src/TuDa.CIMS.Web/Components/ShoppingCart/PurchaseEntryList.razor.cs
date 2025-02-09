using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

/// <summary>
/// A Blazor component, that visualizes <see cref="PurchaseEntry"/>.
/// This uses MudBlazor's <see cref="MudDataGrid{T}"/>.
/// </summary>
public partial class PurchaseEntryList : ComponentBase
{
    /// <summary>
    /// A list of <see cref="PurchaseEntry"/> that will be shown in the list.
    /// </summary>
    [Parameter]
    public required PurchaseResponseDto Purchase { get; set; }

    [Parameter]
    public EventCallback<AssetItem> AssetItemDeleted { get; set; }

    private readonly IDialogService _dialogService;

    /// <summary>
    /// Constructor of <see cref="PurchaseEntryList"/>.
    /// </summary>
    /// <param name="dialogService">Instance of MudBlazor's <see cref="DialogService"/>.</param>
    public PurchaseEntryList(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    /// <summary>
    /// Removes an entry when the user accept the dialog.
    /// </summary>
    /// <param name="entry">The entry to be removed.</param>
    private async Task RemoveEntry(PurchaseEntry entry)
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
            Purchase.Entries.Remove(entry);
            await AssetItemDeleted.InvokeAsync();
        }
    }
}
