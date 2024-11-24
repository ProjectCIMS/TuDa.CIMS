namespace TuDa.CIMS.Shared.Attributes.ServiceRegistration;

public abstract class ServiceRegistrationAttribute(
    Type? serviceType = null,
    Type? implementationType = null,
    string? serviceScopeKey = null
) : Attribute
{
    public Type? ServiceType { get; init; } = serviceType;
    public Type? ImplementationType { get; init; } = implementationType;
    public string? ServiceScopeKey { get; init; } = serviceScopeKey;
}
