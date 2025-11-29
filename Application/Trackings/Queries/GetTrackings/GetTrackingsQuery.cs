using Audita360.Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Audita360.Application.Trackings.Queries.GetTrackings
{
    public class GetTrackingsQuery : IRequest<List<TrackingDto>>
    {
        public int? FindingId { get; set; }    
        public TrackingStatus? Estado { get; set; } 
    }

    public class GetTrackingsQueryHandler : IRequestHandler<GetTrackingsQuery, List<TrackingDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetTrackingsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrackingDto>> Handle(GetTrackingsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Trackings
                .Include(t => t.Finding)  
                .ThenInclude(f => f.Audit) 
                .AsQueryable();

            // filtros
            if (request.FindingId.HasValue)
                query = query.Where(t => t.FindingId == request.FindingId.Value);

            if (request.Estado.HasValue)
                query = query.Where(t => t.Estado == request.Estado.Value);

            return await query
                .OrderBy(t => t.FechaCompromiso)
                .Select(t => new TrackingDto
                {
                    Id = t.Id,
                    Descripcion = t.Descripcion,
                    FechaCompromiso = t.FechaCompromiso,
                    Estado = t.Estado,
                    FindingId = t.FindingId,
                    FindingDescripcion = t.Finding.Descripcion,
                    AuditId = t.Finding.AuditId,
                    AuditTitulo = t.Finding.Audit.Titulo,
                    Created = t.Created,
                    PuedeCambiarEstado = t.Estado != TrackingStatus.Completado 
                })
                .ToListAsync(cancellationToken);
        }
    }

    public class TrackingDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCompromiso { get; set; }
        public TrackingStatus Estado { get; set; }
        public int FindingId { get; set; }
        public string FindingDescripcion { get; set; } = string.Empty;
        public int AuditId { get; set; }
        public string AuditTitulo { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public bool PuedeCambiarEstado { get; set; } 
    }
}