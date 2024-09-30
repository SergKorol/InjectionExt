using InjectionExt.Models;

namespace InjectionExt.Services.Contracts;

public interface IUsersService
{
    Task<Guid> Register(string userName, string password);
    Task<User> GetUserById(Guid id);
}
