using TuDa.CIMS.Api.Errors;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Api.Services;

[ScopedService]
public class ConsumableTransactionService : IConsumableTransactionService
{
    private readonly IConsumableTransactionRepository _consumableTransactionRepository;

    public ConsumableTransactionService(
        IConsumableTransactionRepository consumableTransactionRepository
    )
    {
        _consumableTransactionRepository = consumableTransactionRepository;
    }

    /// <summary>
    /// Return an an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetAllAsync"/> functionality if successful.
    /// </summary>
    public async Task<ErrorOr<List<ConsumableTransaction>>> GetAllAsync()
    {
        try
        {
            return await _consumableTransactionRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            return Error.Failure(
                "ConsumableTransaction.GetAllAsync",
                $"Failed to get all ConsumableTransactions. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetOneAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the ConsumableTransaction</param>
    public async Task<ErrorOr<ConsumableTransaction>> GetOneAsync(Guid id)
    {
        try
        {
            return await _consumableTransactionRepository.GetOneAsync(id) switch
            {
                null => Error.NotFound(
                    "ConsumableTransaction.GetOneAsync",
                    $"ConsumableTransaction with id {id} not found."
                ),
                var value => value,
            };
        }
        catch (Exception e)
        {
            return Error.Unexpected(
                "ConsumableTransaction.GetOneAsync",
                $"An unexpected error occurred: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="CreateAsync"/> functionality if successful
    /// </summary>
    /// <param name="consumableTransactionDto"></param>
    public async Task<ErrorOr<ConsumableTransaction>> CreateAsync(
        CreateConsumableTransactionDto consumableTransactionDto
    )
    {
        try
        {
            return await _consumableTransactionRepository.CreateAsync(consumableTransactionDto);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "ConsumableTransaction.CreateAsync",
                $"Failed to update amount of specific Consumable with Id {consumableTransactionDto.ConsumableId} and failed to create ConsumableTransaction. Exception: {e.Message}"
            );
        }
    }

    public async Task<ErrorOr<Created>> CreateForPurchaseAsync(Purchase purchase)
    {
        try
        {
            if (purchase.CompletionDate is null)
            {
                return PurchaseError.NotCompleted(purchase.Id);
            }

            foreach (var createDto in CreateDtosFromPurchase(purchase))
            {
                var transaction = await CreateAsync(createDto);
                if (transaction.IsError)
                {
                    return transaction.Errors;
                }

                purchase.ConsumableTransactions.Add(transaction.Value);
            }
            return Result.Created;
        }
        catch (Exception e)
        {
            return Error.Failure(
                "ConsumableTransaction.CreateForPurchaseAsync",
                $"Failed to create ConsumableTransaction for the purchase. Exception: {e.Message}"
            );
        }
    }

    private static IEnumerable<CreateConsumableTransactionDto> CreateDtosFromPurchase(
        Purchase purchase
    ) =>
        purchase
            .Entries.Where(e => e.AssetItem is Consumable)
            .GroupBy(entry => entry.AssetItem.Id)
            .Select(group => new CreateConsumableTransactionDto()
            {
                ConsumableId = group.First().AssetItem.Id,
                Date = purchase.CompletionDate!.Value,
                AmountChange = (int)-group.Sum(entry => entry.Amount),
                TransactionReason = TransactionReasons.Purchase,
            });
}
