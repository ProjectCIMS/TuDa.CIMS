using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Dtos.Update;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Extensions;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupPersonList : ComponentBase
{
    [Parameter]
    public required WorkingGroupResponseDto WorkingGroup { get; set; }

    [Parameter]
    public EventCallback<Person> PersonDeleted { get; set; }

    [Parameter]
    public EventCallback PersonAdded { get; set; }

    private readonly IDialogService _dialogService;
    private readonly IStudentApi _studentApi;
    private readonly ISnackbar _snackbar;

    private List<Student> _students => WorkingGroup.Students;

    public WorkingGroupPersonList(
        IDialogService dialogService,
        IStudentApi studentApi,
        ISnackbar snackbar
    )
    {
        _dialogService = dialogService;
        _studentApi = studentApi;
        _snackbar = snackbar;
    }

    /// <summary>
    /// Updates a specific student.
    /// </summary>
    /// <param name="student">The student that will be updated.</param>
    private async Task EditBuyer(Student student)
    {
        var result = await _dialogService.OpenGenericInputPopupAsync(
            "Person bearbeiten",
            [
                new("Vorname", student.FirstName),
                new("Nachname", student.Name, true),
                new("Telefonnummer", student.PhoneNumber),
                new("E-Mail-Adresse", student.Email),
            ],
            "Speichern"
        );

        if (result is null)
            return;

        student.FirstName = result[0];
        student.Name = result[1];
        student.PhoneNumber = result[2];
        student.Email = result[3];

        var updatedStudent = await _studentApi.UpdateAsync(
            WorkingGroup.Id,
            student.Id,
            new UpdateStudentDto()
            {
                FirstName = result[0],
                Name = result[1],
                PhoneNumber = result[2],
                Email = result[3],
            }
        );

        if (updatedStudent.IsError)
        {
            _snackbar.Add("Beim Speichervorgang ist ein Fehler aufgetreten", Severity.Error);
        }
        else
        {
            _snackbar.Add("Erfolgreich gespeichert", Severity.Success);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Removes a student when the user accept the dialog.
    /// </summary>
    /// <param name="student">Student that will be removed</param>
    private async Task RemoveBuyer(Student student)
    {
        var messageBox = new MessageBoxOptions
        {
            Title = "Person löschen",
            Message = "Wollen sie die Person wirklich löschen?",
            YesText = "Löschen",
            NoText = "Nein",
        };

        if (await _dialogService.ShowMessageBox(messageBox) is false)
            return;

        if (_students.Count != 0)
        {
            var deleted = await _studentApi.RemoveAsync(WorkingGroup.Id, student.Id);

            if (deleted.IsError)
            {
                _snackbar.Add("Beim Löschen ist ein Fehler aufgetreten", Severity.Error);
            }
            else
            {
                // Have to be like this otherwise the list will only update after reload
                _students.Remove(student);
                await PersonDeleted.InvokeAsync();
                await InvokeAsync(StateHasChanged);

                _snackbar.Add("Die Person wurde erfolgreich entfernt", Severity.Success);
            }
        }
    }

    /// <summary>
    /// Adds a student to the list of students.
    /// </summary>
    private async Task AddBuyerDialog()
    {
        var result = await _dialogService.OpenGenericInputPopupAsync(
            "Person hinzufügen",
            [new("Vorname"), new("Nachname", true), new("Telefonnummer"), new("E-Mail-Adresse")],
            "Hinzufügen"
        );
        if (result is null)
            return;

        var newStudent = await _studentApi.AddAsync(
            WorkingGroup.Id,
            new CreateStudentDto()
            {
                FirstName = result[0],
                Name = result[1],
                PhoneNumber = result[2],
                Email = result[3],
            }
        );

        // Have to be like this otherwise the list will only update after reload
        if (newStudent.IsError)
        {
            _snackbar.Add("Ein Fehler ist aufgetreten", Severity.Error);
        }
        else
        {
            _students.Add(newStudent.Value);
            await PersonAdded.InvokeAsync();
            await InvokeAsync(StateHasChanged);
        }
    }
}
