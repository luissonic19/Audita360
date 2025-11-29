using Audita360.Application.Trackings.Commands.CreateTracking;
using Audita360.Application.Trackings.Commands.UpdateTrackingStatus;
using Audita360.Application.Trackings.Queries.GetTrackings;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Audita360.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrackingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrackingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TrackingDto>>> GetTrackings(
            [FromQuery] int? findingId,
            [FromQuery] TrackingStatus? estado)
        {
            var query = new GetTrackingsQuery
            {
                FindingId = findingId,
                Estado = estado
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateTrackingCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { id = result, message = "Seguimiento creado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromBody] UpdateTrackingStatusCommand command)
        {
            try
            {
                command.Id = id;
                var result = await _mediator.Send(command);
                return Ok(new { message = "Estado actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}