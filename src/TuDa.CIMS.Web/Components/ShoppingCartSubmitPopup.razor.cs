using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components;

public partial class ShoppingCartSubmitPopup
{
    /// <summary>
    /// Parameter for the working group.
    /// </summary>
    [Parameter]
    public WorkingGroup WorkingGroup { get; set; }

    /// <summary>
    /// CascadingParamter MudDialag.
    /// </summary>
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; }


    /// <summary>
    /// Cancels the MudDialog.
    /// </summary>
    private void Cancel() => MudDialog.Cancel();

    /// <summary>
    /// Closes the MudDialog.
    /// </summary>
    private void Submit() => MudDialog.Close(DialogResult.Ok(WorkingGroup));
}
