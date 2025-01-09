using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Web.Components.Dashboard.Dialogs;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class HomePage
{
    [Inject]
    public required IDialogService DialogService { get; set; }

    private async Task OpenDialogAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        await DialogService.ShowAsync<AssetItemDialog>("Create Item", options);
    }
}
