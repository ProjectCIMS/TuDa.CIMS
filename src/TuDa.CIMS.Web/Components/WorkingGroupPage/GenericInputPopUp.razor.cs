﻿using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public record GenericInputField
{
    public string Label { get; }
    public string Value { get; set; } = string.Empty;
    public bool Required { get; }

    public GenericInputField(string label, string? value = null, bool required = false)
    {
        Label = label;
        Value = value ?? string.Empty;
        Required = required;
    }

    public GenericInputField(string label, bool required)
    {
        Label = label;
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

    private MudForm _form { get; set; } = null!;

    private async Task Submit()
    {
        await _form.Validate();
        if (_form.IsValid)
        {
            MudDialog.Close(DialogResult.Ok(Fields.Select(f => f.Value).ToList()));
        }
    }

    private void Cancel() => MudDialog.Cancel();
}
