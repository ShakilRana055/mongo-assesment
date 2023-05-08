using MongoDB.Driver;
using MongoDBAssesmentDataAccess.IService;

namespace MongoDBAssesmentDataAccess.Service
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;
        public BaseService(IMongoDatabase database)
        {
            _collection = database.GetCollection<T>(typeof(T).Name);
        }
        public async Task InsertAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", GetId(entity));
            await _collection.ReplaceOneAsync(filter, entity);
        }
        public async Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> updateDefination)
        {
            await _collection.UpdateOneAsync(filter, updateDefination);
        }
        public async Task DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter);
        }
        public async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(FilterDefinition<T> filter)
        {
            if (filter == null)
                return await _collection.Find(_ => true).ToListAsync();
            return await _collection.Find(filter).ToListAsync();
        }
        private string GetId(T entity)
        {
            var property = typeof(T).GetProperty("Id");
            return (string)property.GetValue(entity);
        }
    }
}
