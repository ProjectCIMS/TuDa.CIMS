namespace TuDa.CIMS.Shared.Attributes.ServiceRegistration;

/// <remarks>
/// Registers service as singleton.
/// </remarks>
/// <inheritdoc/>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class SingletonServiceAttribute(
    Type? serviceType = null,
    Type? implementationType = null,
    string? serviceKey = null
) : ServiceRegistrationAttribute(serviceType, implementationType, serviceKey);
