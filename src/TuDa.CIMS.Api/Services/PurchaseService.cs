using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;

    public PurchaseService(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<ErrorOr<IEnumerable<Purchase>>> GetAllAsync()
    {
        try
        {
            return (await _purchaseRepository.GetAllAsync()).ToErrorOr();
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
    /// <returns></returns>
    public async Task<ErrorOr<Purchase>> GetOneAsync(Guid id)
    {
        try
        {
            return (await _purchaseRepository.GetOneAsync(id)) switch
            {
                null => Error.NotFound("PurchaseService.GetOneAsync", $"Purchase with id {id} not found."),
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
    /// or the result of the <see cref="UpdateAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the purchase</param>
    /// <param name="updateModel">the model containing the updated values for the purchase</param>
    /// <returns></returns>
    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdatePurchaseDto updateModel)
    {
        try
        {
            return (await _purchaseRepository.UpdateAsync(id, updateModel));
        }
        catch (Exception ex)
        {
            return Error.Failure("PurchaseService.UpdateAsync", ex.Message);
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="RemoveAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the purchase</param>
    /// <returns></returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id)
    {
        try
        {
            return await _purchaseRepository.RemoveAsync(id);
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
    /// <param name="createModel">the model containing the created values for the purchase</param>
    /// <returns></returns>
    public async Task<ErrorOr<Created>> CreateAsync(CreatePurchaseDto createModel)
    {
        try
        {
            return (await _purchaseRepository.CreateAsync(createModel));
        }
        catch (Exception ex)
        {
            return Error.Failure("PurchaseService.CreateAsync", ex.Message);
        }
    }
}
