﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Services;

namespace TuDa.CIMS.Web.Components.ShoppingCart;

public partial class ShoppingCartSubmitPopupBuyerSelection(IWorkingGroupApi workingGroupApi)
    : ComponentBase
{
    /// <summary>
    /// CascadingParameter for working group.
    /// </summary>
    [Parameter]
    public WorkingGroup? WorkingGroup { get; set; }

    [Parameter]
    public required Person Buyer { get; set; }

    // [Parameter]
    // public required bool IsValid { get; set; }

    // [Parameter] public EventCallback<bool> OnValidationChanged { get; set; }

    private MudForm form;
    // private bool coerceValue;
    // private EditContext editContext;
    // private bool BuyerIsValid;

    [Parameter]
    public required bool BuyerIsValid { get; set; }

    [Parameter]
    public EventCallback<bool> BuyerIsValidChanged { get; set; }
    // private IEnumerable<string> Validate(string value)
    // {
    //     if (string.IsNullOrWhiteSpace(value))
    //     {
    //         yield return "Ein Käufer muss ausgewählt werden.";
    //     }
    // }
    // public class Choice
    // {
    //     [Required]
    //     public Person SelectedBuyer { get; set;}
    // }
    // private Choice choice = new();
    private async Task ValidateSelection()
    {
        await form.Validate();
        bool isValid = form.IsValid;
        await BuyerIsValidChanged.InvokeAsync(isValid);
    }

    private async Task OnBuyerChanged(Person buyer)
    {
        await ValidateSelection();
        if (BuyerChanged.HasDelegate)
        {
            await BuyerChanged.InvokeAsync(buyer);
        }
    }
    private string ValidateBuyer(Person? value)
    {
        if (value == null)
        {
            return "Ein Käufer muss ausgewählt werden.";
        }
        return "";
    }
    [Parameter]
    public required EventCallback<Person> BuyerChanged { get; set; }

    private async Task FindWorkingGroup()
    {
        WorkingGroup = (await workingGroupApi.GetAsync(WorkingGroup!.Id)).Match(
            value => value,
            _ => null
        );
    }

    /// <summary>
    /// Search for the selection of the student.
    /// </summary>
    private async Task<IEnumerable<Person>> Search(
        string searchText,
        CancellationToken cancellationToken
    )
    {
        await FindWorkingGroup();
        if (WorkingGroup == null)
        {
            return await Task.FromResult<IEnumerable<Person>>([]);
        }

        var result = new List<Person>();
        result.AddRange(
            WorkingGroup.Students.Where(s =>
                string.IsNullOrWhiteSpace(searchText)
                || s.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
            )
        );
        if (WorkingGroup.Professor.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
        {
            result.Add(WorkingGroup.Professor);
        }
        return await Task.FromResult<IEnumerable<Person>>(result);
    }

    /// <summary>
    /// Returns the name of a given student.
    /// </summary>
    private static string ToString(Person buyer) =>
        buyer switch
        {
            null => "",
            _ => $"{buyer.Name} {buyer.FirstName}",
        };
}
