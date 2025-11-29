using Audita360.Application.Interfaces;
using Audita360.Application.Reports.Queries.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Audita360.Application.Reports.Queries.GetAreaDateSummary
{
    public class GetAreaDateSummaryQuery : IRequest<List<AreaDateSummaryDto>>
    {
        public int? Ano { get; set; }
        public int? Mes { get; set; }
        public string? Area { get; set; }
    }

    public class GetAreaDateSummaryQueryHandler : IRequestHandler<GetAreaDateSummaryQuery, List<AreaDateSummaryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAreaDateSummaryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AreaDateSummaryDto>> Handle(GetAreaDateSummaryQuery request, CancellationToken cancellationToken)
        {
            var auditorias = await _context.Audits
                .Include(a => a.Findings)
                    .ThenInclude(f => f.Trackings)
                .Where(a => a.Estado == AuditStatus.Finalizada)
                .ToListAsync(cancellationToken);

            // métricas
            var result = auditorias
                .GroupBy(a => new { a.AreaAuditada, a.FechaFin.Year, a.FechaFin.Month })
                .Select(g => new AreaDateSummaryDto
                {
                    AreaAuditada = g.Key.AreaAuditada,
                    Ano = g.Key.Year,
                    Mes = g.Key.Month,
                    TotalAuditorias = g.Count(),
                    TotalHallazgos = g.Sum(a => a.Findings.Count),
                    HallazgosAlta = g.Sum(a => a.Findings.Count(f => f.Severidad == FindingSeverity.Alta)),
                    HallazgosMedia = g.Sum(a => a.Findings.Count(f => f.Severidad == FindingSeverity.Media)),
                    HallazgosBaja = g.Sum(a => a.Findings.Count(f => f.Severidad == FindingSeverity.Baja)),
                    PorcentajeSeguimientoCompletado = g.Average(a =>
                        a.Findings.Count > 0 ?
                        (decimal)a.Findings.Count(f => f.Trackings.Any(t => t.Estado == TrackingStatus.Completado)) * 100 / a.Findings.Count : 0
                    ),
                    AuditoriasConHallazgosAlta = g.Count(a => a.Findings.Any(f => f.Severidad == FindingSeverity.Alta))
                })
                .ToList();

            if (request.Ano.HasValue)
                result = result.Where(x => x.Ano == request.Ano.Value).ToList();

            if (request.Mes.HasValue)
                result = result.Where(x => x.Mes == request.Mes.Value).ToList();

            if (!string.IsNullOrEmpty(request.Area))
                result = result.Where(x => x.AreaAuditada.Contains(request.Area)).ToList();

            return result
                .OrderBy(x => x.AreaAuditada)
                .ThenBy(x => x.Ano)
                .ThenBy(x => x.Mes)
                .ToList();
        }
    }
}