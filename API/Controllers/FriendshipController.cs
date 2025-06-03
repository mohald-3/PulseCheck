using API.Helper;
using Application.Commands.Friendship;
using Application.DTOs.FriendshipDtos;
using Application.Queries.Friendship;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FriendshipController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Add a friend
        // Post: api/friendship
        [HttpPost("add-friend")]
        public async Task<IActionResult> CreateFriendship([FromBody] FriendshipEditingDto dto)
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new CreateFriendshipCommand(userId, dto.FriendId));
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        // Get your list of friends
        // GET: api/friendship/user/5
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserFriendships()
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new GetUserFriendshipsQuery(userId));

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        // Unfriend someone
        // DELETE: api/friendship/{userId}/{friendId}
        [HttpDelete("Unfriend")]
        public async Task<IActionResult> DeleteFriendship(int friendId)
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new DeleteFriendshipCommand(userId, friendId));

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(new { message = result.Message });
        }
    }
}
