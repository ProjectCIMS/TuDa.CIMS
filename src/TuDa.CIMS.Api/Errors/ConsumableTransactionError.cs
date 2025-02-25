namespace TuDa.CIMS.Api.Errors;

public static class ConsumableTransactionError
{
    public static Error NotFound(Guid transactionId) =>
        Error.NotFound(
            "ConsumableTransaction.NotFound",
            $"ConsumableTransaction with id {transactionId} was not found."
        );

    public static Error AmountChangeNegative() =>
        Error.Validation(
            "ConsumableTransaction.AmountChangeNegative",
            "Negative amount of consumable after a purchase is not possible."
        );
}
