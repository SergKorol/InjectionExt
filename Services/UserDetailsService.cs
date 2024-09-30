using InjectionExt.Models;
using InjectionExt.Registration.Attributes;
using InjectionExt.Services.Contracts;
using MongoDB.Driver;

namespace InjectionExt.Services;

[Singleton]
public class UserDetailsService(IDatabase database) : IUserDetailsService
{
    public Task Register(string firstName, string lastName, string socialSecurityNumber, Guid userId)
        => database.GetCollectionFor<Models.UserDetails>().InsertOneAsync(new(Guid.NewGuid(), userId, firstName, lastName, socialSecurityNumber));
    
    
}
