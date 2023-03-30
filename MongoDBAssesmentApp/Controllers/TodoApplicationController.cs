using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDBAssesmentDataAccess.IService;
using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoApplicationController : ControllerBase
    {
        private readonly ITodoApplicationService _todoService;

        public TodoApplicationController(ITodoApplicationService todoApplication)
        {
            _todoService = todoApplication;
        }
        [HttpGet("List")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _todoService.GetAllAsync());
        }
        [HttpPost("Save")]
        public async Task<IActionResult> Save(ToDoApplicaton users)
        {
            try
            {
                users.Id = ObjectId.GenerateNewId().ToString();
                await _todoService.InsertAsync(users);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        [HttpGet("GetUserTask/{userId}")]
        public async Task<IActionResult> GetUserTask(string userId)
        {
            try
            {
                return Ok(await _todoService.GetUserTaskByUserId(userId));
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPut("MarkTodoItem/{id}/{hasDone}")]
        public async Task<IActionResult> GetUserTask(string id, bool hasDone)
        {
            try
            {
                var result = await _todoService.GetByIdAsync(id);
                if (result == null) return BadRequest("To do Item not found");

                result.HasDone = hasDone;
                await _todoService.UpdateAsync(result);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("FilterTodos/{hasDone}")]
        public async Task<IActionResult> FilterTodos(bool hasDone)
        {
            try
            {
                return Ok(await _todoService.FilterTodosByStatus(hasDone));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
