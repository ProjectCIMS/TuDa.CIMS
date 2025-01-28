using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupHeader(IWorkingGroupApi workingGroupApi) : ComponentBase
{
    [Parameter] public required Professor Professor { get; set; }

    [Parameter] public string ProfessorName { get; set; } = String.Empty;

    [Parameter] public string ProfessorTitle { get; set; } = String.Empty;

    [Parameter] public Guid WorkingGroupId { get; set; }

    [Inject] private IDialogService DialogService { get; set; } = null!;

    /// <summary>
    /// Sets the ProfessorName and Professor properties
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        var workingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorName = workingGroup.Value.Professor.Name;
        ProfessorTitle = workingGroup.Value.Professor.Title;
        Professor = workingGroup.Value.Professor;

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Opens the dialog to edit the professor
    /// </summary>
    public async Task OpenDialogAsync()
    {
        GenericInput field = new() { Labels = ["Titel", "Nachname"], Values = [Professor.Title, Professor.Name], };

        var parameters = new DialogParameters<GenericInputPopUp> { { up => up.Field, field } };

        var options = new DialogOptions { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<GenericInputPopUp>("Professor bearbeiten", parameters, options);

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;
            ProfessorTitle = returnedValues[0];
            ProfessorName = returnedValues[1];

            await workingGroupApi.UpdateAsync(
                WorkingGroupId,
                new UpdateWorkingGroupDto
                {
                    PhoneNumber = "", Professor = new() { Name = ProfessorName, Title = ProfessorTitle }
                }
            );
            StateHasChanged();
        }
    }
}
