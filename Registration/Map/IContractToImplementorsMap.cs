namespace InjectionExt.Registration.Map;

public interface IContractToImplementorsMap
{
    void Feed(IEnumerable<Type>? types);
}
