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
    public Guid WorkingGroupId { get; set; }

    [CascadingParameter]
    public required MudDialogInstance MudDialog { get; set; }

    [Parameter]
    public required WorkingGroupResponseDto WorkingGroup { get; set; } = new()
    {
        Professor = new Professor()
        {
            Address = new Address(),
            FirstName = "",
            Name = "",
        }, PhoneNumber = "", Email = "",
    };

    [Parameter]
    public required Professor ProfessorInfo { get; set; } = new()
    {
        Address = new Address(),
        FirstName = "",
        Name = "",
    };

    [CascadingParameter]
    public string ProfessorName { get; set; } = string.Empty;

    private string StreetAndNumber =>
        $"{ProfessorInfo.Address.Street} {ProfessorInfo.Address.Number}";

    private string FullName => $"{ProfessorInfo.FirstName} {ProfessorInfo.Name}";

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

    protected override async Task OnInitializedAsync()
    {
        var information = await _workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo = information.Value.Professor;
        WorkingGroup = information.Value;
        ProfessorName = WorkingGroup.Professor.Name;
    }

    private void GoBack() => MudDialog.Cancel();

    private async Task SendUpdateRequestAsync(UpdateWorkingGroupDto updateDto)
    {
        var success = await _workingGroupApi.UpdateAsync(WorkingGroupId, updateDto);
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
            { up => up.Fields, [new("Titel", ProfessorInfo.Title)] },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Titel bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        ProfessorInfo.Title = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto() { Professor = new() { Title = ProfessorInfo.Title } }
        );
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

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Professor bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        ProfessorInfo.FirstName = result[0];
        ProfessorInfo.Name = result[1];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto()
            {
                Professor = new()
                {
                    Name = ProfessorInfo.Name,
                    FirstName = ProfessorInfo.FirstName,
                },
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
            { up => up.Fields, [new("Stadt", ProfessorInfo.Address.City)] },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Stadt bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        ProfessorInfo.Address.City = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto()
            {
                Professor = new() { AddressCity = ProfessorInfo.Address.City },
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
                    new("Straße", ProfessorInfo.Address.Street),
                    new("Hausnummer", ProfessorInfo.Address.Number.ToString()),
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

        ProfessorInfo.Address.Street = result[0];
        ProfessorInfo.Address.Number = int.TryParse(result[1], out int number) ? number : 0;

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto()
            {
                Professor = new()
                {
                    AddressStreet = ProfessorInfo.Address.Street,
                    AddressNumber = ProfessorInfo.Address.Number,
                },
            }
        );
    }

    private async Task EditAddressZipNumber()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { up => up.Fields, [new("Postleitzahl", ProfessorInfo.Address.ZipCode)] },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Postleitzahl bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        ProfessorInfo.Address.ZipCode = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto()
            {
                Professor = new() { AddressZipCode = ProfessorInfo.Address.ZipCode },
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
            { up => up.Fields, [new("Email-Adresse", ProfessorInfo.Email)] },
        };

        var dialogReference = await _dialogService.ShowAsync<GenericInputPopUp>(
            "E-Mail bearbeiten",
            parameters,
            _dialogOptions
        );

        var result = await dialogReference.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        ProfessorInfo.Email = result[0];

        await SendUpdateRequestAsync(
            new UpdateWorkingGroupDto() { Professor = new() { Email = ProfessorInfo.Email } }
        );
    }
}
