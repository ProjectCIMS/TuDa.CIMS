using System.Globalization;
using Microsoft.AspNetCore.Components;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Components.Dashboard.Dialogs;

public partial class AssetItemForm
{
    private readonly CultureInfo _de = CultureInfo.GetCultureInfo("de-DE");

    /// <summary>
    /// Input Fields
    /// </summary>
    public string FormName { get; private set; } = string.Empty;
    public string FormShop { get; private set; } = string.Empty;
    public string FormItemNumber { get; private set; } = string.Empty;
    public string FormNote { get; private set; } = string.Empty;
    public double FormPrice { get; private set; }
    public string FormRoomName { get; private set; } = string.Empty;

    [Parameter]
    public bool FormShowError { get; set; }

    private bool PriceIsError => FormShowError && FormPrice <= 0;
    private bool NameIsError => FormShowError && FormName == string.Empty;
    private bool ShopIsError => FormShowError && FormShop == string.Empty;
    private bool ItemNumberIsError => FormShowError && FormItemNumber == string.Empty;
    private bool IsError => PriceIsError || NameIsError || ShopIsError || ItemNumberIsError;

    [Parameter]
    public EventCallback OnReset { get; set; }

    public void SetForm(AssetItem item)
    {
        FormName = item.Name;
        FormNote = item.Note;
        FormPrice = item.Price;
        FormShop = item.Shop;
        FormRoomName = item.Room.Name;
        FormItemNumber = item.ItemNumber;
    }

    /// <summary>
    /// Resets the Input lines
    /// </summary>
    public void ResetInputs()
    {
        FormName = string.Empty;
        FormShop = string.Empty;
        FormItemNumber = string.Empty;
        FormNote = string.Empty;
        FormPrice = 0.0;
        FormRoomName = string.Empty;
        OnReset.InvokeAsync();
    }

    /// <summary>
    /// Validate the Inputs
    /// </summary>
    /// <returns>returns false when all Inputs are valid otherwise true</returns>
    public bool ErrorsInForm()
    {
        if (IsError)
        {
            return true;
        }

        return false;
    }
}
