using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;
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
        Dictionary<string, List<string>> field = new();

        field.Add("Values", new List<string> { ProfessorName, ProfessorTitle  });
        field.Add("DialogTitle", new List<string>{ "Professor bearbeiten" });
        field.Add("Label", new List<string> { "Name", "Titel" });

        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { x => x.Field, field }
        };

        var options = new DialogOptions { CloseOnEscapeKey = true };

        var dialogReference = await DialogService.ShowAsync<GenericInputPopUp>("Edit Professor", parameters, options);

        var result = await dialogReference.Result;
        var currentWorkingGroup = await workingGroupApi.GetAsync(WorkingGroupId);

        if (!result!.Canceled)
        {
            var returnedValues = (Dictionary<string, List<string>>) result.Data!;
            var valuesList = returnedValues["Values"];
            var name = valuesList[0];
            var title = valuesList[1];

            await workingGroupApi.UpdateAsync(WorkingGroupId,
                new UpdateWorkingGroupDto
                {
                    PhoneNumber = "",
                    Professor = currentWorkingGroup.Value.Professor with { Name = name, Title = title}
                });

            StateHasChanged();
        }
    }
}
