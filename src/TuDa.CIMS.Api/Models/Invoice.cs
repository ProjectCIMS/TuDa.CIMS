using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Models;

/// <summary>
/// Represents an invoice with details about the billing period, professor, and various entries.
/// </summary>
public record Invoice
{
    /// <summary>
    /// The start date of the billing period.
    /// </summary>
    public required DateOnly BeginDate { get; init; }

    /// <summary>
    /// The end date of the billing period.
    /// </summary>
    public required DateOnly EndDate { get; init; }

    /// <summary>
    /// The professor associated with the invoice.
    /// </summary>
    public required Professor Professor { get; init; }

    #region Entries

    /// <summary>
    /// The list of consumable entries in the invoice.
    /// </summary>
    public List<InvoiceEntry> Consumables { get; init; } = [];

    /// <summary>
    /// The list of chemical entries in the invoice.
    /// </summary>
    public List<InvoiceEntry> Chemicals { get; init; } = [];

    /// <summary>
    /// The list of solvent entries in the invoice.
    /// </summary>
    public List<InvoiceEntry> Solvents { get; init; } = [];

    /// <summary>
    /// The list of gas cylinder entries in the invoice.
    /// </summary>
    public List<InvoiceEntry> GasCylinders { get; init; } = [];

    #endregion

    #region Methods

    /// <summary>
    /// Gets the total price of all entries in the invoice.
    /// </summary>
    public double TotalPrice() =>
        ConsumablesTotalPrice()
        + ChemicalsTotalPrice()
        + SolventsTotalPrice()
        + GasCylindersTotalPrice();

    /// <summary>
    /// Gets the total price of all consumable entries in the invoice.
    /// </summary>
    public double ConsumablesTotalPrice() =>
        Consumables.Aggregate(0.0, (state, entry) => state + entry.TotalPrice);

    /// <summary>
    /// Gets the total price of all chemical entries in the invoice.
    /// </summary>
    public double ChemicalsTotalPrice() =>
        Chemicals.Aggregate(0.0, (state, entry) => state + entry.TotalPrice);

    /// <summary>
    /// Gets the total price of all solvent entries in the invoice.
    /// </summary>
    public double SolventsTotalPrice() =>
        Solvents.Aggregate(0.0, (state, entry) => state + entry.TotalPrice);

    /// <summary>
    /// Gets the total price of all gas cylinder entries in the invoice.
    /// </summary>
    public double GasCylindersTotalPrice() =>
        GasCylinders.Aggregate(0.0, (state, entry) => state + entry.TotalPrice);

    #endregion
}
