using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class GenericInputPopUp : ComponentBase
{
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }

    [Parameter] public GenericInput Field { get; set; } = new GenericInput();

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(Field.Values));
    }

    private void Cancel() => MudDialog.Cancel();
}

public class GenericInput
{
    public List<string> Labels { get; set; } = new();
    public List<string> Values { get; set; } = new();

    public string YesText { get; set; } = "Speichern";
}
