using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentDataAccess.IService
{
    public interface IPGTodoApplicationService
    {
        Task<ToDoApplicaton> GetByIdAsync(int id);
        Task<ToDoApplicaton> UpdateAsync(ToDoApplicaton toDoApplicaton);
        Task<IEnumerable<ToDoApplicaton>> GetAllAsync();
        Task<List<ToDoApplicaton>> GetUserTaskByUserId(int userId);
        Task<List<ToDoApplicaton>> FilterTodosByStatus(bool hasDone);
        Task<int> InsertAsync(ToDoApplicaton toDoApplicaton);
    }
}
