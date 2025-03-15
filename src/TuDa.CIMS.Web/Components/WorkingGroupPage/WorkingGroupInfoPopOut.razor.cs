using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Extensions;
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

    private async Task EditProfessorTitle()
    {
        var result = await _dialogService.OpenGenericInputPopupAsync(
            "Titel bearbeiten",
            [new("Titel", Professor.Title)]
        );
        if (result is null)
            return;

        Professor.Title = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto() { Professor = new() { Title = Professor.Title } }
        );
    }

    private async Task EditProfessor()
    {
        var result = await _dialogService.OpenGenericInputPopupAsync(
            "Professor bearbeiten",
            [new("Vorname", Professor.FirstName), new("Nachname", Professor.Name, true)]
        );
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
        var result = await _dialogService.OpenGenericInputPopupAsync(
            "Telefonnummer bearbeiten",
            [new("Telefonnummer", WorkingGroup.PhoneNumber)]
        );
        if (result is null)
            return;

        WorkingGroup.PhoneNumber = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto() { PhoneNumber = WorkingGroup.PhoneNumber }
        );
    }

    private async Task EditAddressCity()
    {
        var result = await _dialogService.OpenGenericInputPopupAsync(
            "Stadt bearbeiten",
            [new("Stadt", Professor.Address.City)]
        );
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
        var result = await _dialogService.OpenGenericInputPopupAsync(
            "Straße und Hausnummer bearbeiten",
            [
                new("Straße", Professor.Address.Street),
                new("Hausnummer", Professor.Address.Number.ToString()),
            ]
        );
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
        var result = await _dialogService.OpenGenericInputPopupAsync(
            "Postleitzahl bearbeiten",
            [new("Postleitzahl", Professor.Address.ZipCode)]
        );
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
        var result = await _dialogService.OpenGenericInputPopupAsync(
            "E-Mail bearbeiten",
            [new("Email-Adresse", WorkingGroup.Email)]
        );
        if (result is null)
            return;

        WorkingGroup.Email = result[0];

        await SendUpdateRequestAsync(new UpdateWorkingGroupDto() { Email = WorkingGroup.Email });
    }

    private async Task EditProfessorEmail()
    {
        var result = await _dialogService.OpenGenericInputPopupAsync(
            "Professor E-Mail bearbeiten",
            [new("Email-Adresse", Professor.Email)]
        );
        if (result is null)
            return;

        Professor.Email = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto() { Professor = new() { Email = Professor.Email } }
        );
    }

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
}
