using InjectionExt.Registration.Attributes;
using InjectionExt.Registration.Types;

namespace InjectionExt.Registration;

public static class ServiceCollectionExtensions
{
    public static void AddBindingsByConvention(this IServiceCollection services, ITypes types)
    {
        if (types.All == null) return;
        {
            var conventionBasedTypes = types.All.Where(t =>
            {
                var interfaces = t.GetInterfaces();
                if (interfaces.Length <= 0) return false;
                var conventionInterface = interfaces.SingleOrDefault(i => Convention(i, t));
                if (conventionInterface != default)
                {
                    return types.All.Count(type => type.HasInterface(conventionInterface)) == 1;
                }
                return false;
            });
    
            foreach (var conventionBasedType in conventionBasedTypes)
            {
                var interfaceToBind = types.All.Single(t => t.IsInterface && Convention(t, conventionBasedType));
                if (services.Any(d => d.ServiceType == interfaceToBind))
                {
                    continue;
                }
    
                if (conventionBasedType.HasAttribute<SingletonAttribute>())
                {
                    _ = services.AddSingleton(interfaceToBind, conventionBasedType);
                }
                else if (conventionBasedType.HasAttribute<ScopedAttribute>())
                {
                    _ = services.AddScoped(interfaceToBind, conventionBasedType);
                }
                else
                {
                    _ = services.AddTransient(interfaceToBind, conventionBasedType);
                }
            }
        }
    
        return;
    
        bool Convention(Type i, Type t) => i.Assembly.FullName == t.Assembly.FullName && i.Name == $"I{t.Name}";
    }
}
