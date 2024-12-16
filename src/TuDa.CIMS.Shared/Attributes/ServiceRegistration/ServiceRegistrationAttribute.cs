namespace TuDa.CIMS.Shared.Attributes.ServiceRegistration;

/// <summary>
/// Automatically registers classes that have an interface or interfaces with this attribute.
/// The registration searches for the interface that is assignable to the class
/// or classes that are assignable from the interface, the attribute is applied on.
/// </summary>
/// <param name="serviceType">The type used for retrieving from the Dependency Injection (DI) container.</param>
/// <param name="implementationType">The type used for the implementation of the service.</param>
/// <param name="serviceKey">The key used for keyed services.</param>
public abstract class ServiceRegistrationAttribute(
    Type? serviceType = null,
    Type? implementationType = null,
    string? serviceKey = null
) : Attribute
{
    public Type? ServiceType { get; init; } = serviceType;
    public Type? ImplementationType { get; init; } = implementationType;
    public string? ServiceKey { get; init; } = serviceKey;
}
