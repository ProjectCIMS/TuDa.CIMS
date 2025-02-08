using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.Dashboard.Dialogs;

public partial class ConsumableItemForm
{
    /// <summary>
    /// Input Fields
    /// </summary>
    public string FormManufacturer { get; private set; } = string.Empty;
    public string FormSerialNumber { get; private set; } = string.Empty;
    public int FormConsumableAmount { get; private set; }

    [Parameter]
    public bool FormShowError { get; set; }

    private bool IsError => FormShowError && FormConsumableAmount <= 0;

    public void SetForm(Consumable item)
    {
        FormManufacturer = item.Manufacturer;
        FormSerialNumber = item.SerialNumber;
        FormConsumableAmount = item.Amount;
    }

    /// <summary>
    /// Validate the Inputs
    /// </summary>
    /// <returns>returns false when all Inputs are valid otherwise true</returns>
    public bool ErrorsInForm()
    {
        if (FormConsumableAmount <= 0)
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
        FormManufacturer = string.Empty;
        FormSerialNumber = string.Empty;
        FormConsumableAmount = 0;
        OnReset.InvokeAsync();
    }
}
