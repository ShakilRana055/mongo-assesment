using MongoDB.Driver;
using MongoDBAssesmentDataAccess.IService;
using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentDataAccess.Service
{
    public class UserService : BaseService<Users>, IUserService
    {
        public UserService(IMongoDatabase database) : base(database)
        {
            
        }
        public async Task<bool> CheckIfUserExist(string username, string email)
        {
            try
            {
                var userNameFilter = Builders<Users>.Filter.Eq("UserName", username);
                var userNameResult = await _collection.Find(userNameFilter).SingleOrDefaultAsync();
                if (userNameResult != null) { return true; }

                var emailFilter = Builders<Users>.Filter.Eq("Email", email);
                var emailResult = await _collection.Find(emailFilter).SingleOrDefaultAsync();
                if (emailResult != null) { return true; }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
