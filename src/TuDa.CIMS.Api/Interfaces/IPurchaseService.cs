using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseService
{
    /// <summary>
    /// Returns all purchases of a working group.
    /// </summary>
    /// <param name="workingGroupId">The id of a working group</param>
    /// <returns>
    /// Success: A list of all purchases in the working group.
    /// Error: An error message. (e.g. if the working group does not exist)
    /// </returns>
    Task<ErrorOr<List<Purchase>>> GetAllAsync(Guid workingGroupId);

    /// <summary>
    /// Get a single purchase by its id.
    /// </summary>
    /// <param name="workingGroupId">the id of a working group of the purchase</param>
    /// <param name="purchaseId">The id of the purchase</param>
    /// <returns>
    /// Success: The purchase with the specified id.
    /// Error: An error message. (e.g. if the purchase does not exist)
    /// </returns>
    Task<ErrorOr<Purchase>> GetOneAsync(Guid workingGroupId, Guid purchaseId);

    /// <summary>
    /// Remove a purchase from the database.
    /// </summary>
    /// <param name="workingGroupId">The id of the working group of the purchase.</param>
    /// <param name="purchaseId">The id of the purchase to remove.</param>
    /// <returns>
    /// Success: The purchase was successfully removed.
    /// Error: An error message. (e.g. if the purchase does not exist)
    /// </returns>
    Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid purchaseId);

    /// <summary>
    /// Create a new purchase.
    /// </summary>
    /// <param name="workingGroupId">The id of the working group of the purchase.</param>
    /// <param name="createModel">The purchase to create.</param>
    /// <returns>
    /// Success: The purchase was successfully created.
    /// Error: An error message. (e.g. if the working group does not exist)
    /// </returns>
    Task<ErrorOr<Purchase>> CreateAsync(Guid workingGroupId, CreatePurchaseDto createModel);

    /// <summary>
    /// Retrieve the signature of a purchase and return it as a string.
    /// </summary>
    /// <param name="workingGroupId">The id of the working group of the purchase.</param>
    /// <param name="purchaseId">The id of the purchase.</param>
    /// <returns>
    /// Success: The signature of the purchase as a string.
    /// Error: An error message. (e.g. if the purchase does not exist)
    /// </returns>
    Task<ErrorOr<string>> RetrieveSignatureAsync(Guid workingGroupId, Guid purchaseId);

    /// <summary>
    /// Invalidate a purchase and correct it with a new one.
    /// </summary>
    /// <param name="workingGroupId">The id of the workingGroup of the purchase.</param>
    /// <param name="purchaseId">The id of the purchase to invalidate.</param>
    /// <param name="createModel">The purchase to correct the old one.</param>
    /// <returns>
    /// Success: The purchase was successfully invalidated.
    /// Error: An error message. (e.g. if the purchase or working group does not exist)
    /// </returns>
    public Task<ErrorOr<Success>> InvalidateAsync(
        Guid workingGroupId,
        Guid purchaseId,
        CreatePurchaseDto createModel
    );
}
