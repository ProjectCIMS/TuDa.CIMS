using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupInfoPopOut(IWorkingGroupApi workingGroupApi) : ComponentBase
{
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [CascadingParameter]
    public required MudDialogInstance MudDialog { get; set; }

    private string StreetAndNumber =>
        $"{ProfessorInfo.Address.Street} {ProfessorInfo.Address.Number}";

    private string FullName => $"{ProfessorInfo.FirstName} {ProfessorInfo.Name}";

    [Parameter]
    public WorkingGroupResponseDto WorkingGroup { get; set; } =
        new()
        {
            Professor = new Professor()
            {
                Address = new Address(),
                FirstName = "",
                Name = "",
                Email = ""
            },
            Students = new List<Student>(),
            PhoneNumber = "",
            Email = "",
            Purchases = new List<PurchaseResponseDto>(),
        };

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Parameter]
    public Professor ProfessorInfo { get; set; } =
        new()
        {
            Address = new Address(),
            FirstName = "",
            Name = "",
        };

    [CascadingParameter]
    public string ProfessorName { get; set; } = String.Empty;

    protected override async Task OnInitializedAsync()
    {
        var information = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo = information.Value.Professor;
        WorkingGroup = information.Value;
        ProfessorName = WorkingGroup.Professor.Name;
    }

    private void GoBack() => MudDialog.Cancel();

    private async Task EditProfessorTitle()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Titel", ProfessorInfo.Title)] },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference = await DialogService.ShowAsync<GenericInputPopUp>(
            "Titel bearbeiten",
            parameters,
            options
        );

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;
            ProfessorInfo.Title = returnedValues[0];

            await workingGroupApi.UpdateAsync(
                WorkingGroupId,
                new UpdateWorkingGroupDto() { Professor = new() { Title = ProfessorInfo.Title } }
            );
            StateHasChanged();
        }
    }

    private async Task EditProfessor()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            {
                up => up.Fields,
                [new("Vorname", ProfessorInfo.FirstName), new("Nachname", ProfessorInfo.Name, true)]
            },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference = await DialogService.ShowAsync<GenericInputPopUp>(
            "Professor bearbeiten",
            parameters,
            options
        );

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;
            ProfessorInfo.FirstName = returnedValues[0];
            ProfessorInfo.Name = returnedValues[1];

            await workingGroupApi.UpdateAsync(
                WorkingGroupId,
                new UpdateWorkingGroupDto()
                {
                    Professor = new()
                    {
                        Name = ProfessorInfo.Name,
                        FirstName = ProfessorInfo.FirstName,
                    },
                }
            );
            StateHasChanged();
        }
    }

    private async Task EditPhoneNumber()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Telefonnummer", WorkingGroup.PhoneNumber)] },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference = await DialogService.ShowAsync<GenericInputPopUp>(
            "Telefonnummer bearbeiten",
            parameters,
            options
        );

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;
            WorkingGroup.PhoneNumber = returnedValues[0];

            await workingGroupApi.UpdateAsync(
                WorkingGroupId,
                new UpdateWorkingGroupDto() { PhoneNumber = WorkingGroup.PhoneNumber }
            );
            StateHasChanged();
        }
    }

    private async Task EditAddressCity()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Stadt", ProfessorInfo.Address.City)] },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference = await DialogService.ShowAsync<GenericInputPopUp>(
            "Stadt bearbeiten",
            parameters,
            options
        );

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;
            ProfessorInfo.Address.City = returnedValues[0];

            await workingGroupApi.UpdateAsync(
                WorkingGroupId,
                new UpdateWorkingGroupDto()
                {
                    Professor = new() { AddressCity = ProfessorInfo.Address.City },
                }
            );
        }

        StateHasChanged();
    }

    private async Task EditAddressStreetAndNumber()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            {
                up => up.Fields,

                [
                    new("Straße", ProfessorInfo.Address.Street),
                    new("Hausnummer", ProfessorInfo.Address.Number.ToString()),
                ]
            },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference = await DialogService.ShowAsync<GenericInputPopUp>(
            "Straße und Hausnummer bearbeiten",
            parameters,
            options
        );

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;
            ProfessorInfo.Address.Street = returnedValues[0];
            ProfessorInfo.Address.Number = int.Parse(returnedValues[1]);

            await workingGroupApi.UpdateAsync(
                WorkingGroupId,
                new UpdateWorkingGroupDto()
                {
                    Professor = new()
                    {
                        AddressStreet = ProfessorInfo.Address.Street,
                        AddressNumber = ProfessorInfo.Address.Number,
                    }
                }
            );
            StateHasChanged();
        }
    }

    private async Task EditAddressZipNumber()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Postleitzahl", ProfessorInfo.Address.ZipCode)] },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference = await DialogService.ShowAsync<GenericInputPopUp>(
            "Postleitzahl bearbeiten",
            parameters,
            options
        );

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;
            ProfessorInfo.Address.ZipCode = returnedValues[0];

            await workingGroupApi.UpdateAsync(
                WorkingGroupId,
                new UpdateWorkingGroupDto()
                {
                    Professor = new() { AddressZipCode = ProfessorInfo.Address.ZipCode },
                }
            );
            StateHasChanged();
        }
    }

    private async Task EditAssistanceEmail()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Email-Adresse", WorkingGroup.Email)] },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference = await DialogService.ShowAsync<GenericInputPopUp>(
            "E-Mail bearbeiten",
            parameters,
            options
        );

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;
            WorkingGroup.Email = returnedValues[0];

            await workingGroupApi.UpdateAsync(
                WorkingGroupId,
                new UpdateWorkingGroupDto() { Email = WorkingGroup.Email }
            );
            StateHasChanged();
        }
    }

    private async Task EditProfessorEmail()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Email-Adresse", ProfessorInfo.Email)] },
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference = await DialogService.ShowAsync<GenericInputPopUp>(
            "E-Mail bearbeiten",
            parameters,
            options
        );

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;
            ProfessorInfo.Email = returnedValues[0];

            await workingGroupApi.UpdateAsync(
                WorkingGroupId,
                new UpdateWorkingGroupDto() { Professor = new() { Email = ProfessorInfo.Email } }
        );
            StateHasChanged();
        }
    }
}
