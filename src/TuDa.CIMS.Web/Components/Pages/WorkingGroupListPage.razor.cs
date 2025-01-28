using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Web.Components.WorkingGroupList;
using TuDa.CIMS.Web.Components.WorkingGroupPage;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class WorkingGroupListPage
{
    [Inject] private IDialogService DialogService { get; set; } = null!;

    [Inject] private IWorkingGroupApi _workingGroupApi { get; set; } = null!;

    private WorkingGroupPageWorkingGroupList _workingGroupList = null!;

    private async Task OpenDialogAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        GenericInput field = new()
        {
            Labels =
            [
                "Vorname des Professors", "Nachname des Professors", "Titel des Professors",
                "E-Mail-Adresse", "Telefonnummer"
            ],
            Values = [string.Empty, string.Empty, string.Empty, string.Empty, string.Empty],
            YesText = "Hinzufügen"
        };
        var parameters = new DialogParameters<GenericInputPopUp> { { up => up.Field, field } };
        var dialog = await DialogService.ShowAsync<GenericInputPopUp>(
            "Arbeitsgruppe erstellen",
            parameters,
            options
        );
        var result = await dialog.Result;
        var returnedValues = (List<string>)result!.Data!;
        if (result is { Canceled: false } && returnedValues[0] != string.Empty && returnedValues[1] != string.Empty
            && returnedValues[2] != string.Empty && returnedValues[3] != string.Empty
            && returnedValues[4] != string.Empty)
        {
            CreateProfessorDto professor = new()
            {
                FirstName = returnedValues[0], Name = returnedValues[1], Title = returnedValues[2],
            };
            await _workingGroupApi.CreateAsync(
                new CreateWorkingGroupDto
                {
                    Professor = professor, Email = returnedValues[3], PhoneNumber = returnedValues[4]
                }
            );
            await _workingGroupList.ReloadDataGridAsync();
        }
    }
}
