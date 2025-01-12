﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;

public partial class WorkingGroupEditEmailDialog(IWorkingGroupApi _workingGroupApi) : ComponentBase
{
    [Parameter] public string ProfessorEmailAddress { get; set; }
    [Parameter] public Guid WorkingGroupId { get; set; }

    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }


    public async Task Save()
    {
        // Saves the city

        var currentWorkingGroup = await  _workingGroupApi.GetAsync(WorkingGroupId);
        await _workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
        {
            PhoneNumber = "",
            Professor = new Professor()
            {
                Address = new Address()
                {
                    City = currentWorkingGroup.Value.Professor.Address.City,
                    Street = currentWorkingGroup.Value.Professor.Address.Street,
                    Number = currentWorkingGroup.Value.Professor.Address.Number,
                    ZipCode = currentWorkingGroup.Value.Professor.Address.ZipCode,
                },
                PhoneNumber = currentWorkingGroup.Value.Professor.PhoneNumber,
                Title = currentWorkingGroup.Value.Professor.Title,
                Email = ProfessorEmailAddress,
                Gender = currentWorkingGroup.Value.Professor.Gender,
                FirstName = currentWorkingGroup.Value.Professor.FirstName,
                Name = currentWorkingGroup.Value.Professor.Name
            }
        });
        MudDialog.Close(DialogResult.Ok(ProfessorEmailAddress));
    }

    public void Cancel()
    {
        // Cancel the dialog

        MudDialog.Cancel();
    }
}

