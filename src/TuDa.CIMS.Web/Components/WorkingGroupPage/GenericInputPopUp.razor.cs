using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public record GenericInputField
{
    public string Label { get; init; }
    public string? Value { get; set; }
    public bool Required { get; init; } = false;

    public GenericInputField(string label, string? value = null, bool required = false)
    {
        Label = label;
        Value = value;
        Required = required;
    }
};

public partial class GenericInputPopUp : ComponentBase
{
    [CascadingParameter]
    public required MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public List<GenericInputField> Fields { get; set; } = [];

    [Parameter]
    public string YesText { get; set; } = "Speichern";

    private void Submit() => MudDialog.Close(DialogResult.Ok(Fields.Select(f => f.Value)));

    private void Cancel() => MudDialog.Cancel();
}
