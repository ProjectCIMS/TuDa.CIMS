using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupProfessorEditDialog(IWorkingGroupApi _workingGroupApi) : ComponentBase
{

    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }

    [Parameter] public Guid WorkingGroupId { get; set; }

    public required string ProfessorName { get; set; } = String.Empty;

    private async void Save()
    {

        var currentWorkingGroup = await  _workingGroupApi.GetAsync(WorkingGroupId);
        await _workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
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
        MudDialog.Close(DialogResult.Ok(ProfessorName));
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }
}
