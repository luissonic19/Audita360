using Audita360.Application.Interfaces;
using Audita360.Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Audita360.Application.Findings.Queries.GetFindings
{
    public class GetFindingsQuery : IRequest<List<FindingDto>>
    {
        public int? AuditId { get; set; }        
        public FindingType? Tipo { get; set; }   
        public FindingSeverity? Severidad { get; set; }
    }

    public class GetFindingsQueryHandler : IRequestHandler<GetFindingsQuery, List<FindingDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetFindingsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FindingDto>> Handle(GetFindingsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Findings
                .Include(f => f.Audit)  
                .AsQueryable();

            if (request.AuditId.HasValue)
                query = query.Where(f => f.AuditId == request.AuditId.Value);

            if (request.Tipo.HasValue)
                query = query.Where(f => f.Tipo == request.Tipo.Value);

            if (request.Severidad.HasValue)
                query = query.Where(f => f.Severidad == request.Severidad.Value);

            return await query
                .OrderByDescending(f => f.FechaHallazgo)
                .Select(f => new FindingDto
                {
                    Id = f.Id,
                    Descripcion = f.Descripcion,
                    Tipo = f.Tipo,
                    Severidad = f.Severidad,
                    FechaHallazgo = f.FechaHallazgo,
                    AuditId = f.AuditId,
                    AuditTitulo = f.Audit.Titulo, 
                    PuedeEliminarse = f.PuedeEliminarse()
                })
                .ToListAsync(cancellationToken);
        }
    }

    public class FindingDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public FindingType Tipo { get; set; }
        public FindingSeverity Severidad { get; set; }
        public DateTime FechaHallazgo { get; set; }
        public int AuditId { get; set; }
        public string AuditTitulo { get; set; } = string.Empty; 
        public bool PuedeEliminarse { get; set; } 
    }
}
