﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupPersonList : ComponentBase
{
    private readonly IDialogService dialogService;
    private readonly IWorkingGroupApi workingGroupApi;
    private readonly IStudentApi studentApi;
    private readonly ISnackbar snackbar;

    public WorkingGroupPersonList(
        IDialogService dialogService,
        IWorkingGroupApi workingGroupApi,
        IStudentApi studentApi,
        ISnackbar snackbar
    )
    {
        this.dialogService = dialogService;
        this.workingGroupApi = workingGroupApi;
        this.studentApi = studentApi;
        this.snackbar = snackbar;
    }

    [Parameter]
    public Guid WorkingGroupId { get; set; }

    private IEnumerable<Student> _students = new List<Student>();

    [Parameter]
    public EventCallback<Person> PersonDeleted { get; set; }

    [Parameter]
    public EventCallback PersonAdded { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var workingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        _students = workingGroup.Value.Students;
    }

    /// <summary>
    /// Updates a specific student.
    /// </summary>
    /// <param name="student">The student that will be updated.</param>
    private async Task EditBuyer(Student student)
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            {
                up => up.Fields,
                [
                    new("Vorname", student.FirstName),
                    new("Nachname", student.Name, true),
                    new("Telefonnummer", student.PhoneNumber),
                    new("E-Mail-Adresse", student.Email),
                ]
            },
            { up => up.YesText, "Speichern" },
        };
        var options = new DialogOptions { CloseOnEscapeKey = true };

        var dialogReference = await dialogService.ShowAsync<GenericInputPopUp>(
            "Person bearbeiten",
            parameters,
            options
        );

        var result = await dialogReference.Result;
        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;
            student.FirstName = returnedValues[0];
            student.Name = returnedValues[1];
            student.PhoneNumber = returnedValues[2];
            student.Email = returnedValues[3];

            var updatedStudent = await studentApi.UpdateAsync(
                WorkingGroupId,
                student.Id,
                new UpdateStudentDto()
                {
                    FirstName = returnedValues[0],
                    Name = returnedValues[1],
                    PhoneNumber = returnedValues[2],
                    Email = returnedValues[3],
                }
            );

            if (updatedStudent.IsError)
            {
                snackbar.Add("Beim Speichervorgang ist ein Fehler aufgetreten", Severity.Error);
            }

            StateHasChanged();
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

        if (await dialogService.ShowMessageBox(messageBox) == true)
        {
            snackbar.Add("Die Person wurde erfolgreich entfernt", Severity.Success);
            if (_students.Any())
            {
                await studentApi.RemoveAsync(WorkingGroupId, student.Id);

                // Have to be like this otherwise the list will only update after reload
                var modifiedList = _students.ToList();
                modifiedList.Remove(student);
                _students = modifiedList;
                await PersonDeleted.InvokeAsync();
            }
        }
    }

    /// <summary>
    /// Adds a student to the list of students.
    /// </summary>
    private async Task AddBuyerDialog()
    {
        var parameters = new DialogParameters<GenericInputPopUp>
        {
            {
                up => up.Fields,
                [new("Vorname"), new("Nachname"), new("Telefonnummer"), new("E-Mail-Adresse")]
            },
            { up => up.YesText, "Hinzufügen" },
        };
        var options = new DialogOptions { CloseOnEscapeKey = true };

        var dialogReference = await dialogService.ShowAsync<GenericInputPopUp>(
            "Person hinzufügen",
            parameters,
            options
        );

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;

            var newStudent = await studentApi.AddAsync(
                WorkingGroupId,
                new CreateStudentDto()
                {
                    FirstName = returnedValues[0],
                    Name = returnedValues[1],
                    PhoneNumber = returnedValues[2],
                    Email = returnedValues[3],
                }
            );

            // Have to be like this otherwise the list will only update after reload
            var modifiableList = _students.ToList();

            if (!newStudent.IsError)
                modifiableList.Add(newStudent.Value);
            else
                snackbar.Add("Ein Fehler ist aufgetreten", Severity.Error);

            _students = modifiableList;
            await PersonAdded.InvokeAsync();
        }
    }
}
