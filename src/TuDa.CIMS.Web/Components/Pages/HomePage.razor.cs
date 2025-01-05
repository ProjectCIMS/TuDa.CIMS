using Microsoft.AspNetCore.Components;
using MudBlazor;

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
