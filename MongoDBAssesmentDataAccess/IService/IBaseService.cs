using MongoDB.Driver;

namespace MongoDBAssesmentDataAccess.IService
{
    public interface IBaseService<T> where T : class
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> updateDefination);
        Task DeleteAsync(string id);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync(FilterDefinition<T> filter = null);
    }
}
