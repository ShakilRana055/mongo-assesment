using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentDataAccess.IService
{
    public interface IPGUserService
    {
        Task<IEnumerable<Users>> GetAllAsync();
        Task<bool> CheckIfUserExist(string username, string email);
        Task<int> InsertAsync(Users users);
        Task<Users> GetByIdAsync(int id);
        Task<Users>UpdateAsync(Users users);
    }
}
