using System.Reflection.Metadata;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TuDa.CIMS.Web.Components.WorkingGroupPage;
    public partial class GenericInputPopUp : ComponentBase
    {

        [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }

        [Parameter] public Dictionary<string, object> Field { get; set; }

        private List<string> _labels = new();
        private List<string> _values = new();

        protected override void OnInitialized()
        {
            if (Field.TryGetValue("Label", out var labelObj) && labelObj is List<string> labels)
            {
                _labels = labels;
            }

            if (Field.TryGetValue("Values", out var valuesObj) && valuesObj is List<string> values)
            {
                _values = values.ToList();
            }
        }

        private void Submit()
        {
            var resultDict = new Dictionary<string, List<string>>
            {
                { "Values", _values }
            };
            MudDialog.Close(DialogResult.Ok(resultDict));
        }

        private void Cancel() => MudDialog.Cancel();
    }
