using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class ShoppingCartSubmitPopupWorkingGroupSelection(IWorkingGroupApi api)
{
    [Parameter] public required WorkingGroup WorkingGroup { get; set; }

    private MudAutocomplete<WorkingGroup> _autocomplete = null!; // Is set by blazor component

    private MudForm form;


    /// <summary>
    /// Event that is called when an <see cref="Shared.Entities.WorkingGroup"/> is selected.
    /// </summary>
    [Parameter]
    public EventCallback<WorkingGroup> WorkingGroupChanged { get; set; }

    [Parameter]
    public required bool WorkingGroupIsValid { get; set; }

    [Parameter]
    public EventCallback<bool> WorkingGroupIsValidChanged { get; set; }

    private async Task OnWorkingGroupChanged(WorkingGroup workingGroup)
    {
        await ValidateSelection();
        if (WorkingGroupChanged.HasDelegate)
        {
            await WorkingGroupChanged.InvokeAsync(workingGroup);
        }
    }

    // private bool coerceValue;
    // private EditContext editContext;
    private string ValidateWorkingGroup(WorkingGroup? value)
    {
        if (value == null)
        {
            return "Eine Arbeitsgruppe muss ausgewählt werden.";
        }
        return "";
    }


    /*private IEnumerable<string> Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            yield return "Eine Arbeitsgruppe muss ausgewählt werden.";
        }
    }
    public class Choice
    {
        [Required]
        public WorkingGroup SelectedWorkingGroup { get; set;}
    }
    private Choice choice = new();

    [Parameter] public EventCallback<bool> OnValidationChanged { get; set; }*/

    private async Task ValidateSelection()
    {
        await form.Validate();
        bool isValid = form.IsValid;
        await WorkingGroupIsValidChanged.InvokeAsync(isValid);
    }

    /// Invoke to clear text
    private async Task WorkingGroupSelectedInternal(WorkingGroup item)
    {
        await _autocomplete.ResetAsync();
        await WorkingGroupChanged.InvokeAsync(item);
    }

    private async Task<IEnumerable<WorkingGroup>> Search(string name, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return [];
        }
        return await api.GetAllAsync(name).Match(value => value, _ => []);
    }

    /// <summary>
    /// Returns the name of a given working group.
    /// </summary>
    private static string ToString(WorkingGroup workingGroup) =>
        workingGroup switch
        {
            null => "",
            _ => workingGroup.Professor.Name,
        };
}
