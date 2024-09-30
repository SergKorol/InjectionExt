using MongoDB.Driver;

namespace InjectionExt.Services.Contracts;

public interface IDatabase
{
    IMongoCollection<T> GetCollectionFor<T>();
}
