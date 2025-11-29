using Audita360.Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAuditsQuery : IRequest<List<AuditDto>>
{
    public AuditStatus? Estado { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? AreaAuditada { get; set; }
    public int? ResponsibleId { get; set; } 
}

public class GetAuditsQueryHandler : IRequestHandler<GetAuditsQuery, List<AuditDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAuditsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AuditDto>> Handle(GetAuditsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Audits
            .Include(a => a.Responsible) 
            .AsQueryable();

        if (request.Estado.HasValue)
            query = query.Where(a => a.Estado == request.Estado.Value);

        if (request.FechaInicio.HasValue)
            query = query.Where(a => a.FechaInicio >= request.FechaInicio.Value);

        if (request.FechaFin.HasValue)
            query = query.Where(a => a.FechaFin <= request.FechaFin.Value);

        if (!string.IsNullOrEmpty(request.AreaAuditada))
            query = query.Where(a => a.AreaAuditada.Contains(request.AreaAuditada));

        if (request.ResponsibleId.HasValue) // <- NUEVO FILTRO
            query = query.Where(a => a.ResponsibleId == request.ResponsibleId.Value);

        return await query
            .OrderByDescending(a => a.FechaInicio)
            .Select(a => new AuditDto
            {
                Id = a.Id,
                Titulo = a.Titulo,
                AreaAuditada = a.AreaAuditada,
                FechaInicio = a.FechaInicio,
                FechaFin = a.FechaFin,
                Estado = a.Estado,
                Created = a.Created,
                ResponsibleId = a.ResponsibleId,
                ResponsibleNombre = a.Responsible.Nombre, 
                ResponsibleCorreo = a.Responsible.Correo  
            })
            .ToListAsync(cancellationToken);
    }
}

public class AuditDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string AreaAuditada { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public AuditStatus Estado { get; set; }
    public DateTime Created { get; set; }
    public int ResponsibleId { get; set; }
    public string ResponsibleNombre { get; set; } = string.Empty; 
    public string ResponsibleCorreo { get; set; } = string.Empty; 
}