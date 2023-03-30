using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDBAssesmentDataAccess.IService;
using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IValidator<Users> _userValidator;
        private readonly IPGUserService _pgUserService;

        public UserController(IUserService userService, IValidator<Users> userValidator, IPGUserService pGUser)
        {
            this.userService = userService;
            _userValidator = userValidator;
            _pgUserService = pGUser;
        }
        [HttpGet("List")]
        public async Task<IActionResult> Get()
        {
            return Ok(await userService.GetAllAsync());
        }
        [HttpPost("Save")]
        public async Task<IActionResult> Save(Users users)
        {
            try
            {
                var result = _userValidator.Validate(users);
                if (!result.IsValid) { return BadRequest(result.Errors); }

                var existingUser = await userService.CheckIfUserExist(users.UserName, users.Email);
                if (existingUser) { return BadRequest("User is already exist"); }

                users.Id = ObjectId.GenerateNewId().ToString();
                await userService.InsertAsync(users);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Users users, string id)
        {
            try
            {
                var exist = await userService.GetByIdAsync(id);
                if (exist != null)
                {
                    users.Id = exist.Id;
                    await userService.UpdateAsync(users);
                    return Ok(users);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
    }
}
