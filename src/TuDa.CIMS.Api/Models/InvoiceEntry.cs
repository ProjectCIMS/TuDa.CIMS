using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Models;

/// <summary>
/// Represents an entry in an invoice, inheriting from PurchaseEntry.
/// </summary>
public record InvoiceEntry : PurchaseEntry
{
    /// <summary>
    /// The date of the entry.
    /// </summary>
    public required DateOnly PurchaseDate { get; init; }

    /// <summary>
    /// The buyer of the entry.
    /// </summary>
    public required Person Buyer { get; init; }
}
