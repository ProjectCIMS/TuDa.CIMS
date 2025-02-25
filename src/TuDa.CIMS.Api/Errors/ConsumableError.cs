namespace TuDa.CIMS.Api.Errors;

public static class ConsumableError
{
    public static Error NotFound(Guid consumableId) =>
        Error.NotFound("Consumable.NotFound", $"Consumable with id {consumableId} was not found.");
}
