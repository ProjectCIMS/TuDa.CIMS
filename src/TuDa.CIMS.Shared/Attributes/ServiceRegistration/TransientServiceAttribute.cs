namespace TuDa.CIMS.Shared.Attributes.ServiceRegistration;

/// <remarks>
/// Registers service as transient.
/// </remarks>
/// <inheritdoc/>
[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class TransientServiceAttribute(
    Type? serviceType = null,
    Type? implementationType = null,
    string? serviceKey = null
) : ServiceRegistrationAttribute(serviceType, implementationType, serviceKey);
