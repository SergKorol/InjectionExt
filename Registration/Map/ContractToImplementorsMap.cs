using System.Collections.Concurrent;
using InjectionExt.Registration.Types;

namespace InjectionExt.Registration.Map;

public class ContractToImplementorsMap : IContractToImplementorsMap
{
    private readonly ConcurrentDictionary<Type, ConcurrentBag<Type>> _contractsAndImplementors = new();
    private readonly ConcurrentDictionary<Type, Type> _allTypes = new();

    public void Feed(IEnumerable<Type>? types)
    {
        if (types == null) return;
        MapTypes(types);
        AddTypesToAllTypes(types);
    }

    private void AddTypesToAllTypes(IEnumerable<Type>? types)
    {
        if (types == null) return;
        foreach (var type in types)
            _allTypes[type] = type;
    }

    private void MapTypes(IEnumerable<Type>? types)
    {
        if (types == null) return;
        var implementors = types.Where(IsImplementation);
        Parallel.ForEach(implementors, implementor =>
        {
            foreach (var contract in implementor.AllBaseAndImplementingTypes())
            {
                var implementingTypes = GetImplementingTypesFor(contract);
                if (!implementingTypes.Contains(implementor)) implementingTypes.Add(implementor);
            }
        });
    }

    private static bool IsImplementation(Type type) => type is { IsInterface: false, IsAbstract: false };

    private ConcurrentBag<Type> GetImplementingTypesFor(Type contract)
    {
        return _contractsAndImplementors.GetOrAdd(contract, _ => new ConcurrentBag<Type>());
    }
}
