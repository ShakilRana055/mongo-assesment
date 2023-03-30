using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentDataAccess.IService
{
    public interface IUserService : IBaseService<Users>
    {
        Task<bool> CheckIfUserExist(string username, string email);
    }
}
