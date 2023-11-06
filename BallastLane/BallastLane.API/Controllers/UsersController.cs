using BallastLane.API.Service;
using BallastLane.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BallastLane.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> Get()
        {
            var (result, clients) = await _userService.GetAllUsers();

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _userService.GetErrorMessage());

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> Get(string id)
        {
            var (result, client) = await _userService.GetUserById(id);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _userService.GetErrorMessage());

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserModel client)
        {
            bool result = await _userService.CreateUser(client);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _userService.GetErrorMessage());

            return Ok();
        }

        [HttpPut()]
        public async Task<ActionResult> Put([FromBody] UserModel client)
        {
            bool result = await _userService.UpdateUser(client);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _userService.GetErrorMessage());

            return Ok();
        }

        [HttpPut("updatepassword")]
        public async Task<ActionResult> Put([FromBody] UpdateUserPasswordModel updateUserPassword)
        {
            bool result = await _userService.UpdateUserPassword(updateUserPassword);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _userService.GetErrorMessage());

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            bool result = await _userService.DeleteUser(id);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _userService.GetErrorMessage());

            return Ok();
        }
    }
}
