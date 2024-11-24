namespace TuDa.CIMS.Shared.Attributes.ServiceRegistration;

[AttributeUsage(AttributeTargets.Interface)]
public class SingletonServiceAttribute(
    Type? serviceType = null,
    Type? implementationType = null,
    string? serviceScopeKey = null
) : ServiceRegistrationAttribute(serviceType, implementationType, serviceScopeKey);
