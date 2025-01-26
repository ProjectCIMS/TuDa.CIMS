using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupInformation(IWorkingGroupApi workingGroupApi) : ComponentBase
{
    [Parameter]
    public Guid WorkingGroupId { get; set; }

    [Parameter]
    public WorkingGroup WorkingGroup { get; set; } =
        new WorkingGroup()
        {
            Professor = new Professor()
            {
                Address = new Address(),
                FirstName = "",
                Name = "",
            },
            Students = new List<Student>(),
            PhoneNumber = "",
            Email = "",
            Purchases = new List<Purchase>(),
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

    protected override async Task OnInitializedAsync()
    {
        var information = await workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo = information.Value.Professor;
        WorkingGroup = information.Value;
    }

    private async Task EditProfessor()
    {
        Dictionary<string, object> field = new()
        {
            { "Values", new List<string> { ProfessorInfo.FirstName, ProfessorInfo.Name } },
            { "DialogTitle", "Professor bearbeiten" },
            { "Label", new List<string> { "Vorname", "Nachname" } }
        };

        var parameters = new DialogParameters<GenericInputPopUp> { { up => up.Field, field } };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<GenericInputPopUp>("Professor bearbeiten", parameters, options);

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
                    PhoneNumber = "",
                    Professor = new() { Name = ProfessorInfo.Name, FirstName = ProfessorInfo.FirstName },
                }
            );
            StateHasChanged();
        }
    }

    private async Task EditPhoneNumber()
    {
        Dictionary<string, object> field = new()
        {
            { "Values", new List<string> { WorkingGroup.PhoneNumber } },
            { "DialogTitle", "Telefonnummer bearbeiten" },
            { "Label", new List<string> { "Telefonnummer" } }
        };

        var parameters = new DialogParameters<GenericInputPopUp> { { up => up.Field, field } };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<GenericInputPopUp>("Telefonnummer bearbeiten", parameters, options);


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
        Dictionary<string, object> field = new()
        {
            { "Values", new List<string> { ProfessorInfo.Address.City } },
            { "DialogTitle", "Stadt bearbeiten" },
            { "Label", new List<string> { "Stadt" } }
        };

        var parameters = new DialogParameters<GenericInputPopUp> { { up => up.Field, field } };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<GenericInputPopUp>("Stadt bearbeiten", parameters, options);

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
        Dictionary<string, object> field = new()
        {
            {
                "Values", new List<string> { ProfessorInfo.Address.Street, ProfessorInfo.Address.Number.ToString() }
            },
            { "DialogTitle", "Straße und Hausnummer bearbeiten" },
            { "Label", new List<string> { "Straße", "Hausnummer" } }
        };

        var parameters = new DialogParameters<GenericInputPopUp> { { up => up.Field, field } };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<GenericInputPopUp>("Straße und Hausnummer bearbeiten", parameters,
                options);

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
                    },
                }
            );
            StateHasChanged();
        }
    }

    private async Task EditAddressZipNumber()
    {
        Dictionary<string, object> field = new()
        {
            { "Values", new List<string> { ProfessorInfo.Address.ZipCode } },
            { "DialogTitle", "Postleitzahl bearbeiten" },
            { "Label", new List<string>{"Postleitzahl"} }
        };

        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Field, field }

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<GenericInputPopUp>("Postleitzahl bearbeiten", parameters, options);


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

    private async Task EditEmail()
    {
        Dictionary<string, object> field = new()
        {
            { "Values", new List<string> { WorkingGroup.Email } },
            { "DialogTitle", "E-Mail bearbeiten" },
            { "Label", new List<string>{"E-Mail-Adresse"} }
        };

        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Field, field }
        };

        var options = new DialogOptions() { CloseOnEscapeKey = true };

        var dialogReference =
            await DialogService.ShowAsync<GenericInputPopUp>("E-Mail bearbeiten", parameters, options);


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
}
