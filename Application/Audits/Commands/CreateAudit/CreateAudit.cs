using Audita360.Application.Interfaces;
using Audita360.Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Audita360.Application.Audits.Commands.CreateAudit
{
    public class CreateAuditCommand : IRequest<int>
    {
        public string Titulo { get; set; } = string.Empty;
        public string AreaAuditada { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int ResponsibleId { get; set; } 
        public AuditStatus Estado { get; set; } = AuditStatus.Pendiente;
    }

    public class CreateAuditCommandHandler : IRequestHandler<CreateAuditCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateAuditCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateAuditCommand request, CancellationToken cancellationToken)
        {
            // Validar que el ResponsibleId existe
            var responsible = await _context.Responsibles
                .FindAsync(new object[] { request.ResponsibleId }, cancellationToken);

            if (responsible == null)
            {
                throw new Exception($"No se encontró un responsable con ID {request.ResponsibleId}");
            }

            var entity = new Audit
            {
                Titulo = request.Titulo,
                AreaAuditada = request.AreaAuditada,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                ResponsibleId = request.ResponsibleId,
                Estado = request.Estado,
                Created = DateTime.UtcNow,
                CreatedBy = "system"
            };

            _context.Audits.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}