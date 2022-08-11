using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.User;
using Pushfi.Application.User.Commands;

namespace Pushfi.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<UsersResponseModel>> GetAll([FromHeader] UsersCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var user = this._userService.GetById(id);
            return Ok(user);
        }
    }
}