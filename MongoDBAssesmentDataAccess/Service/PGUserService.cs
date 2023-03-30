using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using MongoDBAssesmentDataAccess.IService;
using MongoDBAssesmentDomain.Entity;
using Npgsql;

namespace MongoDBAssesmentDataAccess.Service
{
    public class PGUserService : IPGUserService
    {
        private readonly string _connectionString;

        public PGUserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostGreSQLConnection");
        }
        public async Task<IEnumerable<Users>> GetAllAsync()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var userList = new List<Users>();

                var query = @"select * from users;";
                var command = new NpgsqlCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    // something to do...
                    userList.Add(new Users()
                    {
                        Id = Convert.ToString(reader.GetInt64(0)),
                        UserName = reader.GetString(1).ToString(),
                        FirstName = reader.GetString(2).ToString(),
                        LastName = reader.GetString(3).ToString(),
                        Email = reader.GetString(4).ToString(),
                        DateOfBirth = Convert.ToDateTime(reader.GetDateTime(5)),
                    });
                }
                return userList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> CheckIfUserExist(string username, string email)
        {
            try
            {
                var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var query = @"SELECT * FROM users WHERE email = '" + email + "' or username = '" + username + "';";
                var command = new NpgsqlCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();
                var result = await reader.ReadAsync();
                if(result) return true;
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> InsertAsync(Users user)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var query = @"INSERT INTO users(username, firstname, lastname, email, dateofbirth)
                                values(@username, @firstname, @lastname, @email, @dateofbirth)";
                
                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", user.UserName);
                command.Parameters.AddWithValue("@firstname", user.FirstName);
                command.Parameters.AddWithValue("@lastname", user.LastName);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@dateofbirth", user.DateOfBirth);
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Users> GetByIdAsync(int id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var user = new Users(); 

                var query = @"select * from users where id = "+id+";";
                var command = new NpgsqlCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    // something to do...
                    user = new Users()
                    {
                        Id = Convert.ToString(reader.GetInt64(0)),
                        UserName = reader.GetString(1).ToString(),
                        FirstName = reader.GetString(2).ToString(),
                        LastName = reader.GetString(3).ToString(),
                        Email = reader.GetString(4).ToString(),
                        DateOfBirth = Convert.ToDateTime(reader.GetDateTime(5)),
                    };
                }
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Users> UpdateAsync(Users users)
        {
            
             try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var query = @"Update users set firstname = '"+users.FirstName+"', lastname = '"+users.LastName+"' where id ="+Convert.ToInt64(users.Id)+";";
                var command = new NpgsqlCommand(query, connection);
                await command.ExecuteNonQueryAsync();
                return users;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
