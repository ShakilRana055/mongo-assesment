using Microsoft.Extensions.Configuration;
using MongoDBAssesmentDataAccess.IService;
using MongoDBAssesmentDomain.Entity;
using Npgsql;

namespace MongoDBAssesmentDataAccess.Service
{
    public class PGTodoApplicationService : IPGTodoApplicationService
    {
        private readonly string _connectionString;

        public PGTodoApplicationService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostGreSQLConnection");
        }
        public async Task<IEnumerable<ToDoApplicaton>> GetAllAsync()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var toDoApplicatonList = new List<ToDoApplicaton>();

                var query = @"select * from todoapplication;";
                var command = new NpgsqlCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    toDoApplicatonList.Add(new ToDoApplicaton()
                    {
                        Id = Convert.ToString(reader.GetInt64(0)),
                        Name = reader.GetString(1).ToString(),
                        UserId = Convert.ToString(reader.GetInt64(2)),
                        HasDone = reader.GetBoolean(3)
                    });
                }
                return toDoApplicatonList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<int> InsertAsync(ToDoApplicaton toDoApplicaton)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var query = @"insert into todoapplication(name, userid, hasdone)
                                values(@name, @userid, @hasdone)";

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", toDoApplicaton.Name);
                command.Parameters.AddWithValue("@userid", Convert.ToInt64(toDoApplicaton.UserId));
                command.Parameters.AddWithValue("@hasdone", toDoApplicaton.HasDone);
                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ToDoApplicaton>> GetUserTaskByUserId(int userId)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var toDoApplicatonList = new List<ToDoApplicaton>();

                var query = @"select * from todoapplication where userid = "+userId+";";
                var command = new NpgsqlCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    toDoApplicatonList.Add(new ToDoApplicaton()
                    {
                        Id = Convert.ToString(reader.GetInt64(0)),
                        Name = reader.GetString(1).ToString(),
                        UserId = Convert.ToString(reader.GetInt64(2)),
                        HasDone = reader.GetBoolean(3)
                    });
                }
                return toDoApplicatonList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ToDoApplicaton> GetByIdAsync(int id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var toDoApplicaton = new ToDoApplicaton();

                var query = @"select * from todoapplication where id = "+id+";";
                var command = new NpgsqlCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    toDoApplicaton = new ToDoApplicaton()
                    {
                        Id = Convert.ToString(reader.GetInt64(0)),
                        Name = reader.GetString(1).ToString(),
                        UserId = Convert.ToString(reader.GetInt64(2)),
                        HasDone = reader.GetBoolean(3)
                    };
                }
                return toDoApplicaton;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ToDoApplicaton> UpdateAsync(ToDoApplicaton toDoApplicaton)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var query = @"Update todoapplication set hasdone = '" + toDoApplicaton.HasDone + "' where id =" + Convert.ToInt64(toDoApplicaton.Id) + ";";
                var command = new NpgsqlCommand(query, connection);
                await command.ExecuteNonQueryAsync();
                return toDoApplicaton;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<ToDoApplicaton>> FilterTodosByStatus(bool hasDone)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                var toDoApplicatonList = new List<ToDoApplicaton>();

                var query = @"select * from todoapplication where hasdone = " + hasDone + ";";
                var command = new NpgsqlCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    toDoApplicatonList.Add(new ToDoApplicaton()
                    {
                        Id = Convert.ToString(reader.GetInt64(0)),
                        Name = reader.GetString(1).ToString(),
                        UserId = Convert.ToString(reader.GetInt64(2)),
                        HasDone = reader.GetBoolean(3)
                    });
                }
                return toDoApplicatonList;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
