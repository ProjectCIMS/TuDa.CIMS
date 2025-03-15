using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupInfoPopOut : ComponentBase
{
    [Parameter]
    public required WorkingGroupResponseDto WorkingGroup { get; set; }

    [CascadingParameter]
    public required MudDialogInstance MudDialog { get; set; }

    private Professor Professor => WorkingGroup.Professor;
    private string ProfessorName => WorkingGroup.Professor.Name;
    private string StreetAndNumber => $"{Professor.Address.Street} {Professor.Address.Number}";
    private string FullName => $"{Professor.FirstName} {Professor.Name}";

    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true };

    private readonly IDialogService _dialogService;
    private readonly IWorkingGroupApi _workingGroupApi;
    private readonly ISnackbar _snackbar;

    public WorkingGroupInfoPopOut(
        IWorkingGroupApi workingGroupApi,
        IDialogService dialogService,
        ISnackbar snackbar
    )
    {
        _workingGroupApi = workingGroupApi;
        _dialogService = dialogService;
        _snackbar = snackbar;
    }

    private void GoBack() => MudDialog.Cancel();

    private async Task SendUpdateRequestAsync(UpdateWorkingGroupDto updateDto)
    {
        var success = await _workingGroupApi.UpdateAsync(WorkingGroup.Id, updateDto);
        if (success.IsError)
        {
            _snackbar.Add("Etwas ist schief gelaufen", Severity.Error);
        }
        else
        {
            _snackbar.Add("Erfolgreich aktualisiert", Severity.Success);
            StateHasChanged();
        }
    }

    private async Task EditProfessorTitle()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Titel", Professor.Title)] },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Titel bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        Professor.Title = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto() { Professor = new() { Title = Professor.Title } }
        );
    }

    private async Task EditProfessor()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            {
                up => up.Fields,
                [new("Vorname", Professor.FirstName), new("Nachname", Professor.Name, true)]
            },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Professor bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        Professor.FirstName = result[0];
        Professor.Name = result[1];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto()
            {
                Professor = new() { Name = Professor.Name, FirstName = Professor.FirstName },
            }
        );
    }

    private async Task EditPhoneNumber()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Telefonnummer", WorkingGroup.PhoneNumber)] },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Telefonnummer bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        WorkingGroup.PhoneNumber = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto() { PhoneNumber = WorkingGroup.PhoneNumber }
        );
    }

    private async Task EditAddressCity()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Stadt", Professor.Address.City)] },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Stadt bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        Professor.Address.City = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto()
            {
                Professor = new() { AddressCity = Professor.Address.City },
            }
        );
    }

    private async Task EditAddressStreetAndNumber()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            {
                up => up.Fields,
                [
                    new("Straße", Professor.Address.Street),
                    new("Hausnummer", Professor.Address.Number.ToString()),
                ]
            },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Straße und Hausnummer bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        Professor.Address.Street = result[0];
        Professor.Address.Number = int.TryParse(result[1], out int number) ? number : 0;

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto()
            {
                Professor = new()
                {
                    AddressStreet = Professor.Address.Street,
                    AddressNumber = Professor.Address.Number,
                },
            }
        );
    }

    private async Task EditAddressZipNumber()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Postleitzahl", Professor.Address.ZipCode)] },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Postleitzahl bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        Professor.Address.ZipCode = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto()
            {
                Professor = new() { AddressZipCode = Professor.Address.ZipCode },
            }
        );
    }

    private async Task EditAssistanceEmail()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Email-Adresse", WorkingGroup.Email)] },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "E-Mail bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        WorkingGroup.Email = result[0];

        await SendUpdateRequestAsync(new UpdateWorkingGroupDto() { Email = WorkingGroup.Email });
    }

    private async Task EditProfessorEmail()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Email-Adresse", Professor.Email)] },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "E-Mail bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        Professor.Email = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto() { Professor = new() { Email = Professor.Email } }
        );
    }
}
