using Audita360.Application.Findings.Commands.CreateFinding;
using Audita360.Application.Findings.Commands.DeleteFinding;
using Audita360.Application.Findings.Queries.GetFindings;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Audita360.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FindingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FindingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<FindingDto>>> GetFindings(
            [FromQuery] int? auditId,
            [FromQuery] FindingType? tipo,
            [FromQuery] FindingSeverity? severidad)
        {
            var query = new GetFindingsQuery
            {
                AuditId = auditId,
                Tipo = tipo,
                Severidad = severidad
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateFindingCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { id = result, message = "Hallazgo creado exitosamente" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteFindingCommand { Id = id };
            var result = await _mediator.Send(command);

            return Ok(new { message = "Hallazgo eliminado exitosamente" });
        }
    }
}