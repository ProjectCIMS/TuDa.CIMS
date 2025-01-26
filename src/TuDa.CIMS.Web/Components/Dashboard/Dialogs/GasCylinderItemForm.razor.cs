using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Web.Components.Dashboard.Dialogs;

public partial class GasCylinderItemForm
{
    /// <summary>
    /// Input Fields
    /// </summary>
    public double FormVolume { get; private set; }
    public double FormPressure { get; private set; }
    public MeasurementUnits? FormPriceUnit { get; set; }
    public string FormCas { get; set; } = string.Empty;
    public string FormPurity { get; set; } = string.Empty;

    [Parameter]
    public bool FormShowError { get; set; }

    private bool IsError => FormPriceUnit == null && FormShowError;

    public void SetForm(GasCylinder item)
    {
        FormVolume = item.Volume;
        FormPressure = item.Pressure;
        FormCas = item.Cas;
        FormPurity = item.Purity;
        FormPriceUnit = item.PriceUnit;
    }

    /// <summary>
    /// Validate the Inputs
    /// </summary>
    /// <returns>returns false when all Inputs are valid otherwise true</returns>
    public bool ValidateForm()
    {
        if (
            FormVolume == 0.0
            || FormPressure == 0.0
            || FormCas == string.Empty
            || FormPurity == string.Empty
        )
        {
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
        FormVolume = 0.0;
        FormPressure = 0.0;
        FormCas = string.Empty;
        FormPurity = string.Empty;
        OnReset.InvokeAsync();
    }
}
