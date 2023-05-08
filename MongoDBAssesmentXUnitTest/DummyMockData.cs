using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentXUnitTest
{
    public static class DummyMockData
    {
        public static IEnumerable<Users> GetUserList()
        {
            return new List<Users>()
            {
                new Users() {Id = "234345345", UserName = "testuser", Email = "test@gmail.com", DateOfBirth = new DateTime(), FirstName = "Test", LastName = "User"}
            };
        }
    }
}
