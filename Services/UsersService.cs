using InjectionExt.Models;
using InjectionExt.Registration.Attributes;
using InjectionExt.Services.Contracts;
using MongoDB.Driver;

namespace InjectionExt.Services;

[Singleton]
public class UsersService(IDatabase database) : IUsersService
{
    public async Task<Guid> Register(string userName, string password)
    {
        var user = new User(Guid.NewGuid(), userName, password);
        await database.GetCollectionFor<User>().InsertOneAsync(user);
        return user.Id;
    }
    
    public async Task<User> GetUserById(Guid id)
    {
        var users = database.GetCollectionFor<User>();
        var filter = Builders<User>.Filter.Eq(x => x.Id, id);
        
        return await users.Find(filter).FirstOrDefaultAsync();
    }
}
