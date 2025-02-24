using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class ShoppingCartSubmitPopupWorkingGroupSelection(IWorkingGroupApi api)
{
    [Parameter]
    public required WorkingGroupResponseDto WorkingGroup { get; set; }

    private MudAutocomplete<WorkingGroupResponseDto> _autocomplete = null!; // Is set by blazor component

    private MudForm form = null!;

    /// <summary>
    /// Event that is called when an <see cref="Shared.Entities.WorkingGroup"/> is selected.
    /// </summary>
    [Parameter]
    public EventCallback<WorkingGroupResponseDto> WorkingGroupChanged { get; set; }

    [Parameter]
    public required bool WorkingGroupIsValid { get; set; }

    [Parameter]
    public EventCallback<bool> WorkingGroupIsValidChanged { get; set; }

    private async Task OnWorkingGroupChanged(WorkingGroupResponseDto workingGroup)
    {
        await ValidateSelection();
        if (WorkingGroupChanged.HasDelegate)
        {
            await WorkingGroupChanged.InvokeAsync(workingGroup);
        }
    }

    private string ValidateWorkingGroup(WorkingGroup? value)
    {
        if (value is null)
        {
            return "Eine Arbeitsgruppe muss ausgewählt werden.";
        }
        return "";
    }

    private async Task ValidateSelection()
    {
        await form.Validate();
        bool isValid = form.IsValid;
        if (WorkingGroupIsValidChanged.HasDelegate)
        {
            await WorkingGroupIsValidChanged.InvokeAsync(isValid);
        }
    }

    /// Invoke to clear text
    private async Task WorkingGroupSelectedInternal(WorkingGroupResponseDto item)
    {
        await _autocomplete.ResetAsync();
        await WorkingGroupChanged.InvokeAsync(item);
    }

    private Task<IEnumerable<WorkingGroupResponseDto>> Search(
        string name,
        CancellationToken token
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return [];
        }
        var workingGroups = await api.GetAllAsync(name).Match(value => value, _ => []);
        workingGroups = workingGroups.Where(wg => wg.IsDeactivated == false).ToList();
        return workingGroups;
    }


    /// <summary>
    /// Returns the name of a given working group.
    /// </summary>
    private static string WorkingGroupToString(WorkingGroupResponseDto workingGroup) =>
        workingGroup switch
        {
            null => "",
            _ => workingGroup.Professor.Name,
        };
}
