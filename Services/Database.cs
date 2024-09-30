using Humanizer;
using InjectionExt.Registration.Attributes;
using InjectionExt.Services.Contracts;
using MongoDB.Driver;

namespace InjectionExt.Services;

[Singleton]
public class Database : IDatabase
{
    private readonly IMongoDatabase _mongoDatabase;

    public Database()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _mongoDatabase = client.GetDatabase("TheSystem");
    }

    public IMongoCollection<T> GetCollectionFor<T>() => _mongoDatabase.GetCollection<T>(typeof(T).Name.Pluralize());
}
