namespace TuDa.CIMS.Shared.Attributes.ServiceRegistration;

/// <summary>
/// Automatically registers a Refit client with given base route in a WebApp.
/// </summary>
/// <param name="baseRoute">The base route of the Refit client.</param>
[AttributeUsage(AttributeTargets.Interface)]
public class RefitClientAttribute(string? baseRoute = null) : Attribute
{
    public string? BaseRoute { get; private init; } = baseRoute;
}
