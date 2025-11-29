using Audita360.Application.Audits.Commands.CreateAudit;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Audita360.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuditsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuditDto>>> GetAudits(
            [FromQuery] AuditStatus? estado,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] string? areaAuditada)
        {
            var query = new GetAuditsQuery
            {
                Estado = estado,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                AreaAuditada = areaAuditada
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateAuditCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(new { id = result, message = "Auditoría creada exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}