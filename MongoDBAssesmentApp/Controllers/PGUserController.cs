using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDBAssesmentDataAccess.IService;
using MongoDBAssesmentDataAccess.Service;
using MongoDBAssesmentDomain.Entity;

namespace MongoDBAssesmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PGUserController : ControllerBase
    {
        private readonly IValidator<Users> _userValidator;
        private readonly IPGUserService _pgUserService;

        public PGUserController(IValidator<Users> userValidator, IPGUserService pGUser)
        {
            _userValidator = userValidator;
            _pgUserService = pGUser;
        }
        [HttpGet("List")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _pgUserService.GetAllAsync());
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(Users users)
        {
            try
            {
                var result = _userValidator.Validate(users);
                if (!result.IsValid) { return BadRequest(result.Errors); }

                var existingUser = await _pgUserService.CheckIfUserExist(users.UserName, users.Email);
                if (existingUser) { return BadRequest("User is already exist"); }

                //users.Id = ObjectId.GenerateNewId().ToString();
                await _pgUserService.InsertAsync(users);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Users users, int id)
        {
            try
            {
                var exist = await _pgUserService.GetByIdAsync(id);
                if (exist != null)
                {
                    users.Id = exist.Id;
                    await _pgUserService.UpdateAsync(users);
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
