using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
public partial class WorkingGroupPersonList(IDialogService dialogService, IWorkingGroupApi workingGroupApi, IStudentApi studentApi) : ComponentBase
{

    private IEnumerable<Person> _persons = new List<Person>();

    [Parameter] public Guid WorkingGroupId { get; set; }

    [Parameter] public EventCallback<Person> PersonDeleted { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var workingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        _persons = workingGroup.Value.Students;
    }

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
                var modifiableList = _persons.ToList();
                modifiableList.Remove(student);
                _persons = modifiableList;
                await PersonDeleted.InvokeAsync();
            }
        }
    }


    private void AddBuyer()
    {
        // TODO: Implement AddBuyer with a dialog here
    }

}

