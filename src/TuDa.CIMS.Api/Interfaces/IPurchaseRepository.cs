using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IPurchaseRepository
{
    Task<List<Purchase>> GetAllAsync(Guid workingGroupId);
    Task<Purchase?> GetOneAsync(Guid workingGroupId, Guid id);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid id);
    Task<ErrorOr<Purchase>> CreateAsync(Guid workingGroupId, CreatePurchaseDto createModel);

    /// <summary>
    /// Sets the successor and predecessor fields of two purchases.
    /// Used when invalidating a purchase.
    /// </summary>
    /// <param name="workingGroupId">ID of the working group</param>
    /// <param name="predecessorId">ID of the invalidated purchase</param>
    /// <param name="successorId">ID of the new purchase</param>
    /// <returns>
    /// Success: The operation was successful.
    /// Error: An error message.
    /// </returns>
    Task<ErrorOr<Success>> SetSuccessorAndPredecessorAsync(
        Guid workingGroupId,
        Guid predecessorId,
        Guid successorId
    );

    /// <summary>
    /// Retrieves the signature of a purchase and returns it as a string.
    /// </summary>
    /// <param name="workingGroupId">ID of the working group </param>
    /// <param name="purchaseId">ID of the purchase</param>
    /// <returns>
    /// Success: The signature of the purchase as a string.
    /// Error: An error message.
    /// </returns>
    Task<ErrorOr<string>> RetrieveSignatureAsync(Guid workingGroupId, Guid purchaseId);
}
