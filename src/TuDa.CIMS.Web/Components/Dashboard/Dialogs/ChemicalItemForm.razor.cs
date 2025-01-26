using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.Dashboard.Dialogs;

public partial class ChemicalItemForm
{
    /// <summary>
    /// Input Fields
    /// </summary>
    public MeasurementUnits FormPriceUnit { get; private set; }
    public string FormCas { get; private set; } = string.Empty;
    public string FormPurity { get; private set; } = string.Empty;
    public double FormBindingSize { get; private set; }

    [Parameter]
    public bool FormShowError { get; set; }

    private bool IsError =>
        FormShowError
        && (FormCas == string.Empty || FormPurity == string.Empty || FormBindingSize <= 0);

    public void SetForm(Chemical item)
    {
        FormCas = item.Cas;
        FormPurity = item.Purity;
        FormBindingSize = item.BindingSize;
        FormPriceUnit = item.PriceUnit;
    }

    /// <summary>
    /// Validate the Inputs
    /// </summary>
    /// <returns>returns false when all Inputs are valid otherwise true</returns>
    public bool ValidateForm()
    {
        if (FormCas == string.Empty || FormBindingSize == 0.0 || FormPurity == string.Empty)
        {
            FormShowError = true;
            return true;
        }

        return false;
    }

    [Parameter]
    public EventCallback OnReset { get; set; }

    /// <summary>
    /// Resets the Input lines
    /// </summary>
    public void ResetInputs()
    {
        FormCas = string.Empty;
        FormPurity = string.Empty;
        FormBindingSize = 0.0;
        OnReset.InvokeAsync();
    }
}
