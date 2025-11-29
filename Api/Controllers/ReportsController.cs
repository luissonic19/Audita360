using Audita360.Application.Reports.Queries.GetAreaDateSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Audita360.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Auditor")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("area-date-summary")]
        public async Task<ActionResult> GetAreaDateSummary(
            [FromQuery] int? ano,
            [FromQuery] int? mes,
            [FromQuery] string? area)
        {
            var query = new GetAreaDateSummaryQuery
            {
                Ano = ano,
                Mes = mes,
                Area = area
            };

            var result = await _mediator.Send(query);

            return Ok(new
            {
                TotalRegistros = result.Count,
                Filtros = new { Ano = ano, Mes = mes, Area = area },
                Data = result
            });
        }
    }
}