using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components;

/// <summary>
/// A Blazor component, that visualizes <see cref="PurchaseEntry"/>.
/// This uses MudBlazor's <see cref="MudDataGrid{T}"/>.
/// </summary>
/// <param name="dialogService">Instance of MudBlazor's <see cref="DialogService"/>.</param>
public partial class PurchaseEntryList(IDialogService dialogService) : ComponentBase
{
    /// <summary>
    /// A list of <see cref="PurchaseEntry"/> that will be shown in the list.
    /// </summary>
    [Parameter]
    public required List<PurchaseEntry> Entries { get; set; }

    // ReSharper disable once ReplaceWithPrimaryConstructorParameter
    private readonly IDialogService _dialogService = dialogService;

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
            Entries.Remove(entry);
        }
    }
}
