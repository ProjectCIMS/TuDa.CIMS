using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Services;

[ScopedService]
public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IConsumableTransactionService _consumableTransactionService;
    private readonly IDbTransactionManager _transactionManager;
    private readonly IPurchaseInvalidationService _purchaseInvalidationService;

    public PurchaseService(
        IPurchaseRepository purchaseRepository,
        IConsumableTransactionService consumableTransactionService,
        IDbTransactionManager transactionManager,
        IPurchaseInvalidationService purchaseInvalidationService
    )
    {
        _purchaseRepository = purchaseRepository;
        _consumableTransactionService = consumableTransactionService;
        _transactionManager = transactionManager;
        _purchaseInvalidationService = purchaseInvalidationService;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<ErrorOr<Purchase>> GetOneAsync(Guid workingGroupId, Guid purchaseId)
    {
        try
        {
            return (await _purchaseRepository.GetOneAsync(workingGroupId, purchaseId)) switch
            {
                null => Error.NotFound(
                    "PurchaseService.GetOneAsync",
                    $"Purchase with id {purchaseId} not found."
                ),
                var value => value,
            };
        }
        catch (Exception ex)
        {
            return Error.Unexpected("PurchaseService.GetOneAsync", ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid purchaseId)
    {
        try
        {
            return await _purchaseRepository.RemoveAsync(workingGroupId, purchaseId);
        }
        catch (Exception ex)
        {
            return Error.Failure("PurchaseService.RemoveAsync", ex.Message);
        }
    }

    /// <inheritdoc />
    public Task<ErrorOr<Purchase>> CreateAsync(
        Guid workingGroupId,
        CreatePurchaseDto createModel
    ) =>
        _transactionManager.RunInTransactionAsync(async () =>
        {
            var purchase = await _purchaseRepository.CreateAsync(workingGroupId, createModel);
            if (purchase.IsError)
            {
                return purchase.Errors;
            }

            var errorOrCreated = await _consumableTransactionService.CreateForPurchaseAsync(
                purchase.Value
            );

            return errorOrCreated.IsError ? errorOrCreated.Errors : purchase;
        });

    /// <inheritdoc />
    public Task<ErrorOr<Success>> InvalidateAsync(
        Guid workingGroupId,
        Guid purchaseId,
        CreatePurchaseDto createModel
    ) =>
        _transactionManager.RunInTransactionAsync<Success>(async () =>
        {
            var oldPurchase = await _purchaseRepository.GetOneAsync(workingGroupId, purchaseId);
            if (oldPurchase is null)
                return Error.NotFound(
                    "PurchaseRepository.InvalidAsync",
                    $"Purchase {purchaseId} of working group {workingGroupId} was not found."
                );

            if (oldPurchase.Invalidated)
                return Error.Failure(
                    "PurchaseRepository.InvalidateAsync",
                    "Purchase is already invalidated."
                );

            var newPurchase = await _purchaseRepository.CreateAsync(workingGroupId, createModel);
            if (newPurchase.IsError)
                return newPurchase.Errors;

            var success = await _purchaseRepository.SetSuccessorAndPredecessorAsync(
                workingGroupId,
                purchaseId,
                newPurchase.Value.Id
            );
            if (success.IsError)
                return success.Errors;

            var updated = await _purchaseInvalidationService.UpdateForInvalidatedPurchase(
                workingGroupId,
                oldPurchase.Id,
                newPurchase.Value.Id
            );

            if (updated.IsError)
                return updated.Errors;

            return Result.Success;
        });

    /// <inheritdoc />
    public async Task<ErrorOr<string>> RetrieveSignatureAsync(Guid workingGroupId, Guid purchaseId)
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
