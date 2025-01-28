using Microsoft.AspNetCore.Components;
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

    private IEnumerable<Person> _persons = new List<Person>();

    public WorkingGroupPersonList(IDialogService dialogService, IWorkingGroupApi workingGroupApi, IStudentApi studentApi)
    {
        this.dialogService = dialogService;
        this.workingGroupApi = workingGroupApi;
        this.studentApi = studentApi;
    }

    [Parameter] public Guid WorkingGroupId { get; set; }

    [Parameter] public EventCallback<Person> PersonDeleted { get; set; }

    [Parameter] public EventCallback PersonAdded { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var workingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        _persons = workingGroup.Value.Students;
    }

    /// <summary>
    /// Removes a student when the user accept the dialog.
    /// </summary>
    /// <param name="student">Student that will be removed</param>
    private async Task RemoveBuyer(Person student)
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
            if (_persons.Any())
            {
                await studentApi.RemoveAsync(WorkingGroupId, student.Id);
                _persons.ToList().Remove(student);
                await PersonDeleted.InvokeAsync();
            }
        }
    }

    /// <summary>
    /// Adds a student to the list of students.
    /// </summary>
    private async Task AddBuyerDialog()
    {
        GenericInput inputField = new GenericInput()
        {
            Labels = ["Vorname", "Nachname", "Telefonnummer"], Values = ["", "", ""], YesText = "Hinzufügen"
        };

        var parameters = new DialogParameters<GenericInputPopUp> { { up => up.Field, inputField } };
        var options = new DialogOptions { CloseOnEscapeKey = true };

        var dialogReference =
            await dialogService.ShowAsync<GenericInputPopUp>("Person hinzufügen", parameters, options);

        var result = await dialogReference.Result;

        if (!result!.Canceled)
        {
            var returnedValues = (List<string>)result.Data!;

            Student student = new()
            {
                FirstName = returnedValues[0], Name = returnedValues[1], PhoneNumber = returnedValues[2]
            };

            await studentApi.AddAsync(
                WorkingGroupId,
                new CreateStudentDto()
                {
                    FirstName = returnedValues[0], Name = returnedValues[1], PhoneNumber = returnedValues[2]
                }
            );

            var modifiableList = _persons.ToList();
            modifiableList.Add(student);
            _persons = modifiableList;
            await PersonAdded.InvokeAsync();
        }
    }
}
