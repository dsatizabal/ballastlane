using BallastLane.Data.Models;
using MongoDB.Driver;

namespace BallastLane.Data.Repository
{
    public class BaseMongoRepository<T> : IRepository<T> where T : IEntity
    {
        protected readonly IMongoCollection<T> _collection;

        public BaseMongoRepository(IDatabaseSettings settings, string collectionName)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            return await _collection.Find(item => item.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T item)
        {
            if (item.Id != null)
                item.Id = null; // Let MongoDB engine to autocreate the Id

            await _collection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(T item)
        {
            await _collection.ReplaceOneAsync(i => i.Id == item.Id, item);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(item => item.Id == id);
        }
    }
}
