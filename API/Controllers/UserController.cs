using API.Helper;
using Application.Commands.User;
using Application.Commands.User.Application.Commands.User;
using Application.DTOs.UserDtos;
using Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        // GET: api/user/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetUserById()
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new GetUserByIdQuery(userId));

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        // GET: api/user/filter?...
        // More like an admin tool and SHOULD have authoization role for admin [Authorize(Roles = "Admin")]
        //[Authorize]
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredUsers(
            [FromQuery] string? search,
            [FromQuery] string? sort = "created",
            [FromQuery] string? order = "desc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetFilteredUsersQuery
            {
                Search = search,
                Sort = sort,
                Order = order,
                Page = page,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // PATCH: api/user/update
        [Authorize]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto updatedData)
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new UpdateUserCommand(userId, updatedData));

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        // DELETE: api/user
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> SoftDeleteUser()
        {
            var userId = User.GetUserId();

            var result = await _mediator.Send(new SoftDeleteUserCommand(userId));

            return Ok(result.Data);
        }

        // RefreshToken generator
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

    }
}
