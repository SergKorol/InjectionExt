using InjectionExt.Models;

namespace InjectionExt.Services.Contracts;

public interface IUserDetailsService
{
    Task Register(string firstName, string lastName, string socialSecurityNumber, Guid userId);
}
