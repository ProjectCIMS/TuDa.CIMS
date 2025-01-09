using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.Dashboard;

public partial class WorkingGroupListAddDialog
{
    /// <summary>
    /// CascadingParamter MudDialag.
    /// </summary>
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

    /// <summary>
    /// The created working group.
    /// </summary>
    private WorkingGroup WorkingGroup { get; set; } = new()
    {
        Professor = new Professor
        {
            Name = ProfessorName
        }
    };

    /// <summary>
    /// The created name of the professor.
    /// </summary>
    private static string ProfessorName { get; set; } = string.Empty;

    private MudForm form;

    private string ValidateProfessorName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Der Name des Dozierenden darf nicht leer sein.";
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
    private void Submit(){
        if (InputIsValid)
        {
            MudDialog.Close(DialogResult.Ok(WorkingGroup));
        }
    }

    /// <summary>
    /// Cancels the MudDialog.
    /// </summary>
    private void Cancel() => MudDialog.Cancel();

}
