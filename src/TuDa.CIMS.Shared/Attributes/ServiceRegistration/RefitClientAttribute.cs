namespace TuDa.CIMS.Shared.Attributes.ServiceRegistration;

[AttributeUsage(AttributeTargets.Interface)]
public class RefitClientAttribute(string? baseRoute = null) : Attribute
{
    public string? BaseRoute { get; private init; } = baseRoute;
}
