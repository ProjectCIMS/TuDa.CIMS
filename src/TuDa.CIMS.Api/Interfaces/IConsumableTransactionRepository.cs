using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

/// <summary>
/// Interface for managing consumable transactions.
/// </summary>
public interface IConsumableTransactionRepository
{
    /// <summary>
    /// Retrieves all consumable transactions.
    /// </summary>
    /// <returns>A list of all consumable transactions.</returns>
    Task<List<ConsumableTransaction>> GetAllAsync();

    /// <summary>
    /// Retrieves a specific consumable transaction by its ID.
    /// </summary>
    /// <param name="id">The ID of the consumable transaction.</param>
    /// <returns>The consumable transaction if found, otherwise null.</returns>
    Task<ConsumableTransaction?> GetOneAsync(Guid id);

    /// <summary>
    /// Creates a new consumable transaction and updates the amount of the consumable.
    /// </summary>
    /// <param name="consumableTransactionDto">The DTO containing the details of the consumable transaction to create.</param>
    /// <returns>The created consumable transaction or an error.</returns>
    Task<ErrorOr<ConsumableTransaction>> CreateAsync(
        CreateConsumableTransactionDto consumableTransactionDto
    );

    /// <summary>
    /// Updates the amount of a specific consumable transaction.
    /// </summary>
    /// <param name="consumableTransactionId">The ID of the consumable transaction to update.</param>
    /// <param name="newAmount">The new amount to set.</param>
    /// <returns>
    /// Updated: The consumable transaction was successfully updated.
    /// Error: An error occurred.
    /// </returns>
    Task<ErrorOr<Updated>> UpdateAmountAsync(Guid consumableTransactionId, int newAmount);

    /// <summary>
    /// Removes a consumable transaction from the database.
    /// This also updates the amount of the consumable.
    /// </summary>
    /// <param name="consumableTransactionId">The ID of the consumable transaction to remove.</param>
    /// <returns>
    /// Deleted: The consumable transaction was successfully removed.
    /// Error: An error occurred.
    /// </returns>
    Task<ErrorOr<Deleted>> RemoveAsync(Guid consumableTransactionId);

    /// <summary>
    /// Moves all consumable transactions from a predecessor purchase to a successor purchase.
    /// </summary>
    /// <param name="predecessorPurchaseId">The ID of the predecessor purchase.</param>
    /// <param name="successorPurchaseId">The ID of the successor purchase.</param>
    /// <returns>
    /// Success: The operation was successful.
    /// Error: An error occurred.
    /// </returns>
    Task<ErrorOr<Success>> MoveToSuccessorPurchaseAsync(
        Guid predecessorPurchaseId,
        Guid successorPurchaseId
    );

    /// <summary>
    /// Retrieves all consumable transactions for a specific consumable and optional year.
    /// </summary>
    /// <param name="consumableId">The ID of the consumable.</param>
    /// <param name="year">The optional year to filter transactions.</param>
    /// <returns>List of consumable transactions for the consumable or an error.</returns>
    Task<ErrorOr<List<ConsumableTransaction>>> GetAllOfConsumableAsync(
        Guid consumableId,
        int? year
    );

    /// <summary>
    /// Adds a consumable transaction to a purchase.
    /// </summary>
    /// <param name="purchaseGuid">The ID of the purchase.</param>
    /// <param name="consumableTransactionGuid">The ID of the consumable transaction to add.</param>
    /// <returns>
    /// Success: The operation was successful.
    /// Error: An error occurred.
    /// </returns>
    public Task<ErrorOr<Success>> AddToPurchaseAsync(
        Guid purchaseGuid,
        Guid consumableTransactionGuid
    );
}
