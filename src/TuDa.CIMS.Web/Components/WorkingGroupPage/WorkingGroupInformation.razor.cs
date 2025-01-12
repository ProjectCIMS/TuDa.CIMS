using Microsoft.AspNetCore.Components;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Components.WorkingGroupPage.WorkingGroupDialogs;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;

public partial class WorkingGroupInformation(IWorkingGroupApi _workingGroupApi) : ComponentBase
{
    private Guid WorkingGroupId { get; set; }

    [Inject] private IDialogService DialogService { get; set; } = null!;


    [Parameter] public required Professor ProfessorInfo { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var information = await _workingGroupApi.GetAsync(WorkingGroupId);
        ProfessorInfo = information.Value.Professor;
    }

    private Task EditProfessor()
    {
        return DialogService.ShowAsync<WorkingGroupProfessorEditDialog>();
    }

    private Task EditPhoneNumber()
    {
        return DialogService.ShowAsync<WorkingGroupEditPhoneNumberDialog>();
    }

    public Task EditAddressCity()
    {
        return DialogService.ShowAsync<WorkingGroupEditAddressCityDialog>();
    }

    public Task EditAddressStreetAndNumber()
    {
        return DialogService.ShowAsync<WorkingGroupEditAddressStreetAndNumberDialog>();
    }

    public Task EditAddressZipNumber()
    {
        return DialogService.ShowAsync<WorkingGroupEditZipNumberDialog>();
    }

    public Task EditEmail()
    {
        return DialogService.ShowAsync<WorkingGroupEditEmailDialog>();
    }
}

