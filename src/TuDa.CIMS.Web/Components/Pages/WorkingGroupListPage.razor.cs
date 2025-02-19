using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Web.Components.WorkingGroupList;
using TuDa.CIMS.Web.Components.WorkingGroupPage;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class WorkingGroupListPage
{
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private IWorkingGroupApi _workingGroupApi { get; set; } = null!;

    private WorkingGroupPageWorkingGroupList _workingGroupList = null!;

    private async Task OpenDialogAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        List<GenericInputField> fields =
        [
            new("Vorname des Professors"),
            new("Nachname des Professors"),
            new("Titel des Professors"),
            new("E-Mail-Adresse"),
            new("Telefonnummer"),
        ];

        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { popup => popup.Fields, fields },
            { popup => popup.YesText, "Hinzufügen" },
        };
        var dialog = await DialogService.ShowAsync<GenericInputPopUp>(
            "Arbeitsgruppe erstellen",
            parameters,
            options
        );
        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            var returnedValues = (List<string>)result!.Data!;
            CreateProfessorDto professor =
                new()
                {
                    FirstName = returnedValues[0],
                    Name = returnedValues[1],
                    Title = returnedValues[2],
                };
            await _workingGroupApi.CreateAsync(
                new CreateWorkingGroupDto
                {
                    Professor = professor,
                    Email = returnedValues[3],
                    PhoneNumber = returnedValues[4],
                }
            );
            await _workingGroupList.ReloadDataGridAsync();
        }
    }
}
