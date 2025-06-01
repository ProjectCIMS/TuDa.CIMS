using MudBlazor;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Web.Components.WorkingGroupList;
using TuDa.CIMS.Web.Components.WorkingGroupPage;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.Pages;

public partial class WorkingGroupListPage
{
    private readonly IDialogService _dialogService;
    private readonly IWorkingGroupApi _workingGroupApi;
    private readonly ISnackbar _snackbar;

    private WorkingGroupPageWorkingGroupList _workingGroupList = null!;

    public WorkingGroupListPage(
        IWorkingGroupApi workingGroupApi,
        IDialogService dialogService,
        ISnackbar snackbar
    )
    {
        _workingGroupApi = workingGroupApi;
        _dialogService = dialogService;
        _snackbar = snackbar;
    }

    private async Task OpenDialogAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        List<GenericInputField> fields =
        [
            new("Vorname des Professors"),
            new("Nachname des Professors", true),
            new("Titel des Professors"),
            new("E-Mail-Adresse"),
            new("Telefonnummer"),
        ];

        var parameters = new DialogParameters<GenericInputPopUp>
        {
            { popup => popup.Fields, fields },
            { popup => popup.YesText, "Hinzufügen" },
        };
        var dialog = await _dialogService.ShowAsync<GenericInputPopUp>(
            "Arbeitsgruppe erstellen",
            parameters,
            options
        );

        var result = await dialog.GetReturnValueAsync<List<string>>();
        if (result is null)
            return;

        CreateProfessorDto professor =
            new()
            {
                FirstName = result[0],
                Name = result[1],
                Title = result[2],
            };
        var created = await _workingGroupApi.CreateAsync(
            new CreateWorkingGroupDto
            {
                Professor = professor,
                Email = result[3],
                PhoneNumber = result[4],
            }
        );

        if (created.IsError)
        {
            _snackbar.Add("Etwas ist schief gelaufen", Severity.Error);
        }
        else
        {
            _snackbar.Add("Arbeitsgruppe erfolgreich erstellt", Severity.Success);
            await _workingGroupList.ReloadDataGridAsync();
        }
    }
}
