using Audita360.Application.Responsibles.Commands.CreateResponsible;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Audita360.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ResponsiblesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ResponsiblesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateResponsibleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { id = result, message = "Responsable creado exitosamente" });
        }

        [HttpGet("{id}/audits")]
        public async Task<ActionResult<List<AuditDto>>> GetAuditsByResponsible(int id)
        {
            var query = new GetAuditsQuery { ResponsibleId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}