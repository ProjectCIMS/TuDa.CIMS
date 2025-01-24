using System.Reflection.Metadata;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage
{
    public partial class GenericInputPopUp : ComponentBase
    {
        [Parameter]
        public Dictionary<string, List<string>> Field { get; set; } = new();

        [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }

        public void Save()
        {
            // Saves the input of the fields
            MudDialog.Close(DialogResult.Ok(Field));
        }

        public void Cancel()
        {
            // Cancel the dialog
            MudDialog.Cancel();
        }
    }
}
