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

    [Parameter] public Guid WorkingGroupId { get; set; }

    [Inject] private IDialogService DialogService { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Get the id from the url
    /// </summary>
    private Guid GetIdFromUrl()
    {
        var uri = NavigationManager.Uri.Split('/');
        var idString = uri[^1];
        return Guid.Parse(idString);
    }


    /// <summary>
    /// Sets the ProfessorName and Professor properties
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        var workingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorName = workingGroup.Value.Professor.Name;
        Professor = workingGroup.Value.Professor;

        await base.OnInitializedAsync();
    }

    public void GetWorkingGroupId()
    {
        WorkingGroupId = GetIdFromUrl();
    }

    /// TODO: Add dialog
    public async Task OpenDialogAsync()
    {
        var parameters = new DialogParameters
        {
            { nameof(WorkingGroupProfessorEditDialog.ProfessorName), ProfessorName },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true};

        var dialogReference =
            await DialogService.ShowAsync<WorkingGroupProfessorEditDialog>("Edit Professor", parameters, options);


        var result = await dialogReference.Result;
        var currentWorkingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorName = result.Data.ToString();

        //TODO: Update the working group with the new professor name

        await workingGroupApi.UpdateAsync(WorkingGroupId,
            new UpdateWorkingGroupDto()
            {
                PhoneNumber = "",
                Professor = new Professor()
                {
                    Address = currentWorkingGroup.Value.Professor.Address,
                    PhoneNumber = currentWorkingGroup.Value.Professor.PhoneNumber,
                    Title = currentWorkingGroup.Value.Professor.Title,
                    Email = currentWorkingGroup.Value.Professor.Email,
                    Gender = currentWorkingGroup.Value.Professor.Gender,
                    FirstName = currentWorkingGroup.Value.Professor.FirstName,
                    Name = ProfessorName
                }
            });
    }
}
