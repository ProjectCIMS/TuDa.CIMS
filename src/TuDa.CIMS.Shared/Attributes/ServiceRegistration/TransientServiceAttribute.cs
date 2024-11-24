namespace TuDa.CIMS.Shared.Attributes.ServiceRegistration;

[AttributeUsage(AttributeTargets.Interface)]
public class TransientServiceAttribute(
    Type? serviceType = null,
    Type? implementationType = null,
    string? serviceScopeKey = null
) : ServiceRegistrationAttribute(serviceType, implementationType, serviceScopeKey);
