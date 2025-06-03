using Application.Commands.User;
using Application.Commands.User.Application.Commands.User;
using Application.DTOs.UserDtos;
using Application.Queries.User;
using Application.Services.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UserController(IUserService userService, IMapper mapper, IMediator mediator)
        {
            _userService = userService;
            _mapper = mapper;
            _mediator = mediator;
        }

        // usage would be for finding friends by searching for their email or name or phone number
        // GET: api/user/filter?...
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredUsers(
            [FromQuery] string? search,
            [FromQuery] string? sort = "created",
            [FromQuery] string? order = "desc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var users = await _userService.GetFilteredUsersAsync(search, sort, order, page, pageSize);
            return Ok(users);
        }

        // Done
        // GET: api/user/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetUserById()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim);
            var result = await _mediator.Send(new GetUserByIdQuery(userId));

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        // Done
        // POST: api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Done
        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        // Done
        // PATCH: api/user/{id}
        [Authorize]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto updatedData)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("User ID not found in token.");

            var userId = int.Parse(userIdClaim);
            var result = await _mediator.Send(new UpdateUserCommand(userId, updatedData));

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        // Done
        // DELETE: api/user/{id}
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> SoftDeleteUser()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _mediator.Send(new SoftDeleteUserCommand(userId));

            return Ok(new
            {
                Message = "User deleted successfully.",
                Deleted = true
            });
        }

        // Done
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
