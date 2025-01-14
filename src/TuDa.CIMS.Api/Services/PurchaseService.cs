using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IConsumableTransactionService _consumableTransactionService;

    public PurchaseService(IPurchaseRepository purchaseRepository,IConsumableTransactionService consumableTransactionService )
    {
        _purchaseRepository = purchaseRepository;
        _consumableTransactionService = consumableTransactionService;
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetAllAsync"/> functionality if successful
    /// </summary>
    /// <param name="workingGroupId">the unique id of a workinggroup</param>
    /// <returns></returns>
    public async Task<ErrorOr<IEnumerable<Purchase>>> GetAllAsync(Guid workingGroupId)
    {
        try
        {
            return (await _purchaseRepository.GetAllAsync(workingGroupId)).ToErrorOr();
        }
        catch (Exception ex)
        {
            return Error.Failure("PurchaseService.GetAllAsync", ex.Message);
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetOneAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the purchase</param>
    /// <param name="workingGroupId">the unique id of a workinggroup</param>
    /// <returns></returns>
    public async Task<ErrorOr<Purchase>> GetOneAsync(Guid id, Guid workingGroupId)
    {
        try
        {
            return (await _purchaseRepository.GetOneAsync(id, workingGroupId)) switch
            {
                null => Error.NotFound(
                    "PurchaseService.GetOneAsync",
                    $"Purchase with id {id} not found."
                ),
                var value => value,
            };
        }
        catch (Exception ex)
        {
            return Error.Unexpected("PurchaseService.GetOneAsync", ex.Message);
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="RemoveAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the purchase</param>
    /// <param name="workingGroupId">the unique id of a workinggroup</param>
    /// <returns></returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id, Guid workingGroupId)
    {
        try
        {
            return await _purchaseRepository.RemoveAsync(id, workingGroupId);
        }
        catch (Exception ex)
        {
            return Error.Failure("PurchaseService.RemoveAsync", ex.Message);
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="CreateAsync"/> functionality if successful
    /// </summary>
    /// <param name="workingGroupId">the unique id of a workinggroup</param>
    /// <param name="createModel">the model containing the created values for the purchase</param>
    /// <returns></returns>
    public async Task<ErrorOr<Purchase>> CreateAsync(
        Guid workingGroupId,
        CreatePurchaseDto createModel
    )
    {
        try
        {
            var purchase = await _purchaseRepository.CreateAsync(workingGroupId, createModel);
            if (purchase.IsError)
            {
                return purchase.Errors;
            }
            await _consumableTransactionService.CreateForPurchaseAsync(purchase.Value);
            return purchase;
        }
        catch (Exception ex)
        {
            return Error.Failure("PurchaseService.CreateAsync", ex.Message);
        }
    }

}
