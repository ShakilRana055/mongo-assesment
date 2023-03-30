using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentDataAccess.IService
{
    public interface ITodoApplicationService : IBaseService<ToDoApplicaton>
    {
        Task<List<ToDoApplicaton>> GetUserTaskByUserId(string userId);
        Task<List<ToDoApplicaton>> FilterTodosByStatus(bool hasDone);
    }
}
