using System.Reflection;

namespace InjectionExt.Registration.Types;

public static class TypeExtensions
{

    public static bool HasAttribute<T>(this Type type)
        where T : Attribute
    {
        var attributes = type.GetTypeInfo().GetCustomAttributes(typeof(T), false).ToArray();
        return attributes.Length == 1;
    }

    public static bool HasInterface(this Type type, Type interfaceType)
    {
        if (interfaceType.IsGenericTypeDefinition)
        {
            return type.GetTypeInfo()
                        .ImplementedInterfaces
                        .Count(t =>
                        {
                            if (t.IsGenericType &&
                                interfaceType.GetTypeInfo().GenericTypeParameters.Length == t.GetGenericArguments().Length)
                            {
                                var genericType = interfaceType.MakeGenericType(t.GetGenericArguments());
                                return t == genericType;
                            }
                            return false;
                        }) == 1;
        }

        return type.GetTypeInfo()
                    .ImplementedInterfaces
                    .Count(t => t == interfaceType) == 1;
    }

    public static IEnumerable<Type> AllBaseAndImplementingTypes(this Type type)
    {
        return type.BaseTypes()
            .Concat(type.GetTypeInfo().GetInterfaces())
            .SelectMany(ThisAndMaybeOpenType)
            .Where(t => t != type && t != typeof(object));
    }


    private static IEnumerable<Type> BaseTypes(this Type type)
    {
        var currentType = type;
        while (currentType != null)
        {
            yield return currentType;
            currentType = currentType.GetTypeInfo().BaseType;
        }
    }

    private static IEnumerable<Type> ThisAndMaybeOpenType(Type type)
    {
        yield return type;
        if (type.GetTypeInfo().IsGenericType && !type.GetTypeInfo().ContainsGenericParameters)
        {
            yield return type.GetGenericTypeDefinition();
        }
    }
}
