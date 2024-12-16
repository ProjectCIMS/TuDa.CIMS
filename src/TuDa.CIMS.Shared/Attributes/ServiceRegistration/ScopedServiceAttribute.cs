namespace TuDa.CIMS.Shared.Attributes.ServiceRegistration;

/// <remarks>
/// Registers service as scoped.
/// </remarks>
/// <inheritdoc/>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class ScopedServiceAttribute(
    Type? serviceType = null,
    Type? implementationType = null,
    string? serviceKey = null
) : ServiceRegistrationAttribute(serviceType, implementationType, serviceKey);
