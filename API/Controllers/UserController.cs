using Application.Commands.User;
using Application.DTOs.UserDtos;
using Application.Services.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/user/{id}
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
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

        // PATCH: api/user/{id}
        [Authorize]
        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateDto updatedData)
        {
            var updatedUser = await _userService.UpdateUserAsync(userId, updatedData);
            if (updatedUser == null)
                return NotFound();

            return Ok(updatedUser);
        }

        // DELETE: api/user/{id}
        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> SoftDeleteUser(int userId)
        {
            var success = await _userService.SoftDeleteUserAsync(userId);
            if (!success)
                return NotFound();

            return NoContent(); // 204 — successful but nothing returned
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
