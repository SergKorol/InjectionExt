using System.Reflection;
using InjectionExt.Registration.Map;
using Microsoft.Extensions.DependencyModel;

namespace InjectionExt.Registration.Types;

public class Types : ITypes
{
    private readonly IContractToImplementorsMap _contractToImplementorsMap = new ContractToImplementorsMap();

    public Types(params string[] assemblyPrefixesToInclude)
    {
        All = DiscoverAllTypes(assemblyPrefixesToInclude);
        _contractToImplementorsMap.Feed(All);
    }

    public IEnumerable<Type>? All { get; }

    private static IEnumerable<Type>? DiscoverAllTypes(IEnumerable<string> assemblyPrefixesToInclude)
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly == null) return null;
        var dependencyModel = DependencyContext.Load(entryAssembly);
        var projectReferencedAssemblies = dependencyModel?.RuntimeLibraries
            .Where(l => l.Type.Equals("project"))
            .Select(l =>
            {
                ArgumentNullException.ThrowIfNull(l);
                return Assembly.Load(l.Name);
            })
            .ToArray();
    
        var assemblies = dependencyModel?.RuntimeLibraries
            .Where(l => l.RuntimeAssemblyGroups.Count > 0 &&
                        assemblyPrefixesToInclude.Any(asm => l.Name.StartsWith(asm)))
            .Select(l =>
            {
                ArgumentNullException.ThrowIfNull(l);
                try
                {
                    return Assembly.Load(l.Name);
                }
                catch
                {
                    return null;
                }
            })
            .Where(a => a is not null)
            .Distinct()
            .ToList();
    
        if (projectReferencedAssemblies != null) assemblies?.AddRange(projectReferencedAssemblies);
        return assemblies?.SelectMany(a => a?.GetTypes() ?? Type.EmptyTypes).ToArray();
    }
}
