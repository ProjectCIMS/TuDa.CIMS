using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
public partial class WorkingGroupHeader(IWorkingGroupApi workingGroupApi) : ComponentBase
{
    [Parameter] public required Professor Professor { get; set; }

    [Parameter] public string ProfessorName { get; set; } = String.Empty;

    private Guid WorkingGroupId { get; set; }

    [Inject] private IDialogService DialogService { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Get the id from the url
    /// </summary>
    private Guid GetIdFromUrl()
    {
        var uri = NavigationManager.Uri.Split('/');
        var idString = uri[^1];
        return Guid.Parse(idString);
    }


    /// <summary>
    /// Sets the ProfessorName and Professor properties
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // var workingGroup = await workingGroupApi.GetAsync(WorkingGroupId);
        var workingGroup = new WorkingGroup()
        {
            PhoneNumber = "232323",
            Purchases = new List<Purchase>(),
            Students = new List<Student>(),
            Professor = new Professor()
            {
                Email = "n",
                FirstName = "vr",
                Gender = Gender.Divers,
                Title = "Mk",
                Address = new Address() { City = "Munich", Number = 23, Street = "Street", ZipCode = "12345" },
                Name = "Professor Name"
            }
        };
        ProfessorName = workingGroup.Professor.Name;
        Professor = workingGroup.Professor;

        // await base.OnInitializedAsync();
    }

    public void GetWorkingGroupId()
    {
        WorkingGroupId = GetIdFromUrl();
    }

    /// TODO: Add dialog
    private Task OpenDialogAsync()
    {
        /* var parameters = new DialogParameters<WorkingGroupProfessorEditDialog>
        {
            { x => x.WorkingGroupId, WorkingGroupId },
            { x => x.ProfessorName, ProfessorName }
        }; */
        return DialogService.ShowAsync<WorkingGroupProfessorEditDialog>();
    }
}
