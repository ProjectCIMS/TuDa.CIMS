using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.WorkingGroupList;

public partial class WorkingGroupPageWorkingGroupListAddDialog
{
    /// <summary>
    /// CascadingParameter MudDialog.
    /// </summary>
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// The created name of the professor.
    /// </summary>
    private string ProfessorName { get; set; } = string.Empty;

    private MudForm form = null!;

    private string ValidateProfessorName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Der Name des Professors darf nicht leer sein.";
        }

        InputIsValid = true;
        return "";
    }

    /// <summary>
    /// Returns true if the input is valid.
    /// </summary>
    private bool InputIsValid { get; set; } = false;

    /// <summary>
    /// Closes the MudDialog.
    /// </summary>
    private void Submit()
    {
        if (InputIsValid)
        {
            Professor professor = new() { Name = ProfessorName };
            MudDialog.Close(DialogResult.Ok(professor));
        }
    }

    /// <summary>
    /// Cancels the MudDialog.
    /// </summary>
    private void Cancel() => MudDialog.Cancel();
}
