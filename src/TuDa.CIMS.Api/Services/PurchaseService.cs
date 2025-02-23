using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IConsumableTransactionService _consumableTransactionService;
    private readonly CIMSDbContext _context;

    public PurchaseService(
        IPurchaseRepository purchaseRepository,
        IConsumableTransactionService consumableTransactionService,
        CIMSDbContext context
    )
    {
        _purchaseRepository = purchaseRepository;
        _consumableTransactionService = consumableTransactionService;
        _context = context;
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetAllAsync"/> functionality if successful
    /// </summary>
    /// <param name="workingGroupId">the unique id of a workinggroup</param>
    /// <returns></returns>
    public async Task<ErrorOr<List<Purchase>>> GetAllAsync(Guid workingGroupId)
    {
        try
        {
            return await _purchaseRepository.GetAllAsync(workingGroupId);
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
    public async Task<ErrorOr<Purchase>> GetOneAsync(Guid workingGroupId, Guid id)
    {
        try
        {
            return (await _purchaseRepository.GetOneAsync(workingGroupId, id)) switch
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
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid id)
    {
        try
        {
            return await _purchaseRepository.RemoveAsync(workingGroupId, id);
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
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync<ErrorOr<Purchase>>(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var purchase = await _purchaseRepository.CreateAsync(workingGroupId, createModel);
                if (purchase.IsError)
                {
                    return purchase.Errors;
                }

                var errorOrCreated = await _consumableTransactionService.CreateForPurchaseAsync(purchase.Value);
                if (errorOrCreated.IsError)
                {
                    return errorOrCreated.Errors;
                }

                await transaction.CommitAsync();

                return purchase;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Error.Failure("PurchaseService.CreateAsync", ex.Message);
            }
        });
    }

    public async Task<ErrorOr<Success>> InvalidateAsync(
        Guid workingGroupId,
        Guid purchaseId,
        CreatePurchaseDto createModel
    )
    {
        try
        {
            return await _purchaseRepository.InvalidateAsync(
                workingGroupId,
                purchaseId,
                createModel
            );
        }
        catch (Exception ex)
        {
            return Error.Failure("PurchaseService.InvalidateAsync", ex.Message);
        }
    }

    public async Task<ErrorOr<byte[]>> RetrieveSignatureAsync(Guid workingGroupId, Guid purchaseId)
    {
        try
        {
            return await _purchaseRepository.RetrieveSignatureAsync(workingGroupId, purchaseId);
        }
        catch (Exception ex)
        {
            return Error.Failure("PurchaseService.RetrieveSignatureAsync", ex.Message);
        }
    }

}
