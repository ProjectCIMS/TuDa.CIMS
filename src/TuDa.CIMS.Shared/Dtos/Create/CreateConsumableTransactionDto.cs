using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Shared.Dtos.Create;

public class CreateConsumableTransactionDto
{
    /// <summary>
    /// The consumable of this transaction.
    /// </summary>
    public required Guid ConsumableId { get; set; }

    /// <summary>
    /// The date of the transaction.
    /// </summary>
    public required DateTime Date { get; set; }

    /// <summary>
    /// Represents the change in amount for a Consumable.
    /// </summary>
    /// <remarks>
    /// Negative values indicate removal, positive values indicate addition.
    /// </remarks>
    public required int AmountChange { get; set; }

    /// <summary>
    /// The reason for the transaction.
    /// </summary>
    public required TransactionReasons TransactionReason { get; set; }
}
