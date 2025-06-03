using API.Helper;
using Application.Commands.CheckIn;
using Application.DTOs.CheckInDtos;
using Application.Queries.CheckIn;
using Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInController : ControllerBase
    {
        private readonly ICheckInService _checkInService;
        private readonly IMediator _mediator;

        public CheckInController(ICheckInService checkInService, IMediator mediator)
        {
            _checkInService = checkInService;
            _mediator = mediator;
        }

        // CheckMeIn
        // Post: api/checkin
        [Authorize]
        [HttpPost("CheckMeIn")]
        public async Task<IActionResult> CreateCheckIn()
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new CreateCheckInCommand(userId));

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        // Get my check in histroy
        // GET: api/checkin/me 
        [Authorize]
        [HttpGet("histroy/me")]
        public async Task<IActionResult> GetMyCheckIns()
        {
            var userId = User.GetUserId();
            var result = await _mediator.Send(new GetUserCheckInsQuery(userId));

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        // Get specified check in histroy (can be restricted later)
        // GET: api/checkin/user/5
        [Authorize]
        [HttpGet("histroy/user/{userId}")]
        public async Task<IActionResult> GetCheckInsForUser(int userId)
        {
            var result = await _mediator.Send(new GetUserCheckInsQuery(userId));

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Data);
        }
    }
}
