using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupInformation(IWorkingGroupApi workingGroupApi) : ComponentBase
{
    [Parameter] public Guid WorkingGroupId { get; set; }

    [Inject] private IDialogService DialogService { get; set; } = null!;

    [Parameter] public Professor ProfessorInfo { get; set; } = new Professor()
    {
        Address = new Address(),
        Email = "",
        FirstName = "",
        Name = "lmce"
    };


    protected override async Task OnInitializedAsync()
    {
        var information = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo = information.Value.Professor;
    }

    private async Task EditProfessor()
    {
        var parameters = new DialogParameters
        {
            { nameof(WorkingGroupProfessorEditDialog.ProfessorName), ProfessorInfo.Name },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true};

        var dialogReference =
            await DialogService.ShowAsync<WorkingGroupProfessorEditDialog>("Edit Professor", parameters, options);


        var result = await dialogReference.Result;
        var currentWorkingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo.Name = result.Data.ToString();

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
                    Name = ProfessorInfo.Name
                }
            });
    }

    private async Task EditPhoneNumber()
    {
        var parameters = new DialogParameters
        {
            { nameof(WorkingGroupEditPhoneNumberDialog.ProfessorPhoneNumber), ProfessorInfo.PhoneNumber },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true};

        var dialogReference =
            await DialogService.ShowAsync<WorkingGroupProfessorEditDialog>("Edit PhoneNumber", parameters, options);


        var result = await dialogReference.Result;
        ProfessorInfo.PhoneNumber = result.Data.ToString();

        //TODO: Update the working group with the new professor name
        var currentWorkingGroup = await  workingGroupApi.GetAsync(WorkingGroupId);
        await workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
        {
            PhoneNumber = "ProfessorPhoneNumber",
            Professor = new Professor()
            {
                Address = new Address()
                {
                    City = currentWorkingGroup.Value.Professor.Address.City,
                    Street = currentWorkingGroup.Value.Professor.Address.Street,
                    Number = currentWorkingGroup.Value.Professor.Address.Number,
                    ZipCode = currentWorkingGroup.Value.Professor.Address.ZipCode,
                },
                PhoneNumber = ProfessorInfo.PhoneNumber,
                Title = currentWorkingGroup.Value.Professor.Title,
                Email = currentWorkingGroup.Value.Professor.Email,
                Gender = currentWorkingGroup.Value.Professor.Gender,
                FirstName = currentWorkingGroup.Value.Professor.FirstName,
                Name = currentWorkingGroup.Value.Professor.Name
            }
        });
        }

    private async Task EditAddressCity()
    {
        var parameters = new DialogParameters
        {
            { nameof(WorkingGroupEditAddressCityDialog.ProfessorCity), ProfessorInfo.Address.City },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true};

        var dialogReference =
            await DialogService.ShowAsync<WorkingGroupProfessorEditDialog>("Edit City", parameters, options);


        var result = await dialogReference.Result;
        var currentWorkingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo.Address.City = result.Data.ToString();

        //TODO: Update the working group with the new professor name
        await workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
        {
            PhoneNumber = "",
            Professor = new Professor()
            {
                Address = new Address()
                {
                    City = ProfessorInfo.Address.City,
                    Street = currentWorkingGroup.Value.Professor.Address.Street,
                    Number = currentWorkingGroup.Value.Professor.Address.Number,
                    ZipCode = currentWorkingGroup.Value.Professor.Address.ZipCode,

                },
                PhoneNumber = currentWorkingGroup.Value.Professor.PhoneNumber,
                Title = currentWorkingGroup.Value.Professor.Title,
                Email = currentWorkingGroup.Value.Professor.Email,
                Gender = currentWorkingGroup.Value.Professor.Gender,
                FirstName = currentWorkingGroup.Value.Professor.FirstName,
                Name = currentWorkingGroup.Value.Professor.Name
            }
        });
    }

    private async Task EditAddressStreetAndNumber()
    {
        var parameters = new DialogParameters
        {
            { nameof(WorkingGroupEditAddressStreetAndNumberDialog.ProfessorNumber), ProfessorInfo.Address.Number },
            { nameof(WorkingGroupEditAddressStreetAndNumberDialog.ProfessorStreet), ProfessorInfo.Address.Street }
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<WorkingGroupEditAddressStreetAndNumberDialog>("Edit Street and Number", parameters,
                options);


        var result = await dialogReference.Result;
        var currentWorkingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo.Address.Street = result.Data.ToString();

        //TODO: Update the working group with the new professor name
        await workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
        {
            PhoneNumber = "",
            Professor = new Professor()
            {
                Address = new Address()
                {
                    City = currentWorkingGroup.Value.Professor.Address.City,
                    Street = ProfessorInfo.Address.Street,
                    Number = ProfessorInfo.Address.Number,
                    ZipCode = currentWorkingGroup.Value.Professor.Address.ZipCode,
                },
                PhoneNumber = currentWorkingGroup.Value.Professor.PhoneNumber,
                Title = currentWorkingGroup.Value.Professor.Title,
                Email = currentWorkingGroup.Value.Professor.Email,
                Gender = currentWorkingGroup.Value.Professor.Gender,
                FirstName = currentWorkingGroup.Value.Professor.FirstName,
                Name = currentWorkingGroup.Value.Professor.Name
            }
        });
    }

    private async Task EditAddressZipNumber()
    {
        var parameters = new DialogParameters
        {
            { nameof(WorkingGroupEditZipNumberDialog.ProfessorZipNumber), ProfessorInfo.Address.ZipCode },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<WorkingGroupEditZipNumberDialog>("Edit ZipCode", parameters, options);


        var result = await dialogReference.Result;
        var currentWorkingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo.Address.ZipCode = result.Data.ToString();

        //TODO: Update the working group with the new professor name
        await workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
        {
            PhoneNumber = "",
            Professor = new Professor()
            {
                Address = new Address()
                {
                    City = currentWorkingGroup.Value.Professor.Address.City,
                    Street = currentWorkingGroup.Value.Professor.Address.Street,
                    Number = currentWorkingGroup.Value.Professor.Address.Number,
                    ZipCode = ProfessorInfo.Address.ZipCode
                },
                PhoneNumber = currentWorkingGroup.Value.Professor.PhoneNumber,
                Title = currentWorkingGroup.Value.Professor.Title,
                Email = currentWorkingGroup.Value.Professor.Email,
                Gender = currentWorkingGroup.Value.Professor.Gender,
                FirstName = currentWorkingGroup.Value.Professor.FirstName,
                Name = currentWorkingGroup.Value.Professor.Name
            }
        });
    }

    private async Task EditEmail()
    {
        var parameters = new DialogParameters
        {
            { nameof(WorkingGroupEditEmailDialog.ProfessorEmailAddress), ProfessorInfo.Email },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<WorkingGroupEditEmailDialog>("Edit Email", parameters, options);


        var result = await dialogReference.Result;
        var currentWorkingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo.Email = result.Data.ToString();

        //TODO: Update the working group with the new professor name
        await workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
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
                Email = ProfessorInfo.Email,
                Gender = currentWorkingGroup.Value.Professor.Gender,
                FirstName = currentWorkingGroup.Value.Professor.FirstName,
                Name = currentWorkingGroup.Value.Professor.Name
            }
        });
    }
}
