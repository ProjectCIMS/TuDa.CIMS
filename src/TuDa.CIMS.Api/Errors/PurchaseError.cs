namespace TuDa.CIMS.Api.Errors;

public static class PurchaseError
{
    public static Error NotFound(Guid purchaseId) =>
        Error.NotFound("Purchase.NotFound", $"Purchase with id {purchaseId} was not found.");

    public static Error NotCompleted(Guid purchaseId) =>
        Error.Validation(
            "Purchase.NotCompleted",
            $"Purchase with id {purchaseId} is not completed."
        );
}
