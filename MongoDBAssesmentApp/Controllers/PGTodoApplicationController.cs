using Microsoft.AspNetCore.Mvc;
using MongoDBAssesmentDataAccess.IService;
using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PGTodoApplicationController : ControllerBase
    {
        private readonly IPGTodoApplicationService _pgTodoService;

        public PGTodoApplicationController(IPGTodoApplicationService pgTodoService)
        {
            _pgTodoService = pgTodoService;
        }
        [HttpGet("List")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _pgTodoService.GetAllAsync());
        }
        [HttpPost("Save")]
        public async Task<IActionResult> Save(ToDoApplicaton toDoApplicaton)
        {
            try
            {
                await _pgTodoService.InsertAsync(toDoApplicaton);
                return Ok(toDoApplicaton);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        [HttpGet("GetUserTask/{userId}")]
        public async Task<IActionResult> GetUserTask(int userId)
        {
            try
            {
                return Ok(await _pgTodoService.GetUserTaskByUserId(userId));
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPut("MarkTodoItem/{id}/{hasDone}")]
        public async Task<IActionResult> GetUserTask(int id, bool hasDone)
        {
            try
            {
                var result = await _pgTodoService.GetByIdAsync(id);
                if (result == null) return BadRequest("To do Item not found");

                result.HasDone = hasDone;
                await _pgTodoService.UpdateAsync(result);
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
                return Ok(await _pgTodoService.FilterTodosByStatus(hasDone));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
