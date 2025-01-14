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

    [Parameter] public WorkingGroup WorkingGroup { get; set; } = new WorkingGroup()
    {
        Professor = new Professor()
        {
            Address = new Address(),
            FirstName = "",
            Name = ""
        },
        Students = new List<Student>(),
        PhoneNumber = "",
        Email = "",
        Purchases = new List<Purchase>()
    };

    [Inject] private IDialogService DialogService { get; set; } = null!;

    [Parameter] public Professor ProfessorInfo { get; set; } = new()
    {
        Address = new Address(),
        FirstName = "",
        Name = ""
    };


    protected override async Task OnInitializedAsync()
    {
        var information = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo = information.Value.Professor;
        WorkingGroup = information.Value;
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
        if(!result!.Canceled)
        {
            ProfessorInfo.Name = result.Data!.ToString()!;

            await workingGroupApi.UpdateAsync(WorkingGroupId,
                new UpdateWorkingGroupDto()
                {
                    PhoneNumber = "",
                    Professor = currentWorkingGroup.Value.Professor with {Name = ProfessorInfo.Name}
                });
            StateHasChanged();
        }
    }

    private async Task EditPhoneNumber()
    {
        var parameters = new DialogParameters
        {
            { nameof(WorkingGroupEditPhoneNumberDialog.ProfessorPhoneNumber), WorkingGroup.PhoneNumber },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true};

        var dialogReference =
            await DialogService.ShowAsync<WorkingGroupEditPhoneNumberDialog>("Edit PhoneNumber", parameters, options);


        var result = await dialogReference.Result;
        var currentWorkingGroup = await  workingGroupApi.GetAsync(WorkingGroupId);

        if (!result!.Canceled)
        {
            WorkingGroup.PhoneNumber = result.Data!.ToString()!;

            await workingGroupApi.UpdateAsync(WorkingGroupId,
                new UpdateWorkingGroupDto()
                {
                    PhoneNumber = WorkingGroup.PhoneNumber,
                    Professor = currentWorkingGroup.Value.Professor
                });
            StateHasChanged();
        }
    }

    private async Task EditAddressCity()
    {
        var parameters = new DialogParameters
        {
            { nameof(WorkingGroupEditAddressCityDialog.ProfessorCity), ProfessorInfo.Address.City },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true};

        var dialogReference =
            await DialogService.ShowAsync<WorkingGroupEditAddressCityDialog>("Edit City", parameters, options);


        var result = await dialogReference.Result;
        var currentWorkingGroup = await workingGroupApi.GetAsync(WorkingGroupId);

        if (!result!.Canceled)
        {
            ProfessorInfo.Address.City = result.Data!.ToString()!;

            await workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
            {
                PhoneNumber = "",
                Email = "",
                Professor = new Professor()
                {
                    Address = new Address()
                    {
                        Id = currentWorkingGroup.Value.Professor.Address.Id,
                        City = ProfessorInfo.Address.City,
                        Street = currentWorkingGroup.Value.Professor.Address.Street,
                        Number = currentWorkingGroup.Value.Professor.Address.Number,
                        ZipCode = currentWorkingGroup.Value.Professor.Address.ZipCode,

                    },
                    Id = currentWorkingGroup.Value.Professor.Id,
                    Title = currentWorkingGroup.Value.Professor.Title,
                    Gender = currentWorkingGroup.Value.Professor.Gender,
                    FirstName = currentWorkingGroup.Value.Professor.FirstName,
                    Name = currentWorkingGroup.Value.Professor.Name
                }
            });
        }
        StateHasChanged();
    }

    private async Task EditAddressStreetAndNumber()
    {
        var parameters = new DialogParameters
            // TODO: Fix street and number problem
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

        if (!result!.Canceled)
        {
            ProfessorInfo.Address.Street = result.Data!.ToString()!;

            await workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
            {
                PhoneNumber = "",
                Email = "",
                Professor = new Professor()
                {
                    Address = new Address()
                    {
                        Id = currentWorkingGroup.Value.Professor.Address.Id,
                        City = currentWorkingGroup.Value.Professor.Address.City,
                        Street = ProfessorInfo.Address.Street,
                        Number = ProfessorInfo.Address.Number,
                        ZipCode = currentWorkingGroup.Value.Professor.Address.ZipCode,
                    },
                    Id = currentWorkingGroup.Value.Professor.Id,
                    Title = currentWorkingGroup.Value.Professor.Title,
                    Gender = currentWorkingGroup.Value.Professor.Gender,
                    FirstName = currentWorkingGroup.Value.Professor.FirstName,
                    Name = currentWorkingGroup.Value.Professor.Name
                }
            });
            StateHasChanged();
        }
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
        if (!result!.Canceled)
        {
            ProfessorInfo.Address.ZipCode = result.Data!.ToString()!;

            await workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
            {
                PhoneNumber = "",
                Email = "",
                Professor = new Professor()
                {
                    Address = new Address()
                    {
                        Id = currentWorkingGroup.Value.Professor.Address.Id,
                        City = currentWorkingGroup.Value.Professor.Address.City,
                        Street = currentWorkingGroup.Value.Professor.Address.Street,
                        Number = currentWorkingGroup.Value.Professor.Address.Number,
                        ZipCode = ProfessorInfo.Address.ZipCode
                    },
                    Id = currentWorkingGroup.Value.Professor.Id,
                    Title = currentWorkingGroup.Value.Professor.Title,
                    Gender = currentWorkingGroup.Value.Professor.Gender,
                    FirstName = currentWorkingGroup.Value.Professor.FirstName,
                    Name = currentWorkingGroup.Value.Professor.Name
                }
            });
            StateHasChanged();
        }
    }

    private async Task EditEmail()
    {
        var parameters = new DialogParameters
        {
            { nameof(WorkingGroupEditEmailDialog.ProfessorEmailAddress), WorkingGroup.Email },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<WorkingGroupEditEmailDialog>("Edit Email", parameters, options);


        var result = await dialogReference.Result;
        var currentWorkingGroup = await workingGroupApi.GetAsync(WorkingGroupId);

        if (!result!.Canceled)
        {
            WorkingGroup.Email = result.Data!.ToString()!;

            await workingGroupApi.UpdateAsync(WorkingGroupId, new UpdateWorkingGroupDto()
            {
                PhoneNumber = "",
                Email = WorkingGroup.Email,
                Professor = new Professor()
                {
                    Address = new Address()
                    {
                        Id = currentWorkingGroup.Value.Professor.Address.Id,
                        City = currentWorkingGroup.Value.Professor.Address.City,
                        Street = currentWorkingGroup.Value.Professor.Address.Street,
                        Number = currentWorkingGroup.Value.Professor.Address.Number,
                        ZipCode = currentWorkingGroup.Value.Professor.Address.ZipCode,
                    },
                    Id = currentWorkingGroup.Value.Professor.Id,
                    Title = currentWorkingGroup.Value.Professor.Title,
                    Gender = currentWorkingGroup.Value.Professor.Gender,
                    FirstName = currentWorkingGroup.Value.Professor.FirstName,
                    Name = currentWorkingGroup.Value.Professor.Name
                }
            });
            StateHasChanged();
        }
    }
}
