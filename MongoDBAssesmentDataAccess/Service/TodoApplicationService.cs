using MongoDB.Driver;
using MongoDBAssesmentDataAccess.IService;
using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentDataAccess.Service
{
    public class TodoApplicationService : BaseService<ToDoApplicaton>, ITodoApplicationService
    {
        public TodoApplicationService(IMongoDatabase database) : base(database)
        {
            
        }
        public async Task<List<ToDoApplicaton>> GetUserTaskByUserId(string userId)
        {
            var filter = Builders<ToDoApplicaton>.Filter.Eq("UserId", userId);
            return await _collection.Find(filter).ToListAsync();
        }
        public async Task<List<ToDoApplicaton>> FilterTodosByStatus(bool hasDone)
        {
            var filter = Builders<ToDoApplicaton>.Filter.Eq("HasDone", hasDone);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
