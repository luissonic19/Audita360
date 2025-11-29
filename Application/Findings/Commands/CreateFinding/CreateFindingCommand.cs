using Audita360.Application.Interfaces;
using Audita360.Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Audita360.Application.Findings.Commands.CreateFinding
{
    public class CreateFindingCommand : IRequest<int>
    {
        public string Descripcion { get; set; } = string.Empty;
        public FindingType Tipo { get; set; }
        public FindingSeverity Severidad { get; set; }
        public DateTime FechaHallazgo { get; set; }
        public int AuditId { get; set; } 
    }

    public class CreateFindingCommandHandler : IRequestHandler<CreateFindingCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateFindingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateFindingCommand request, CancellationToken cancellationToken)
        {
            // Verificar que la auditoría existe
            var audit = await _context.Audits.FindAsync(new object[] { request.AuditId }, cancellationToken);
            if (audit == null)
            {
                throw new Exception("Auditoría no encontrada");
            }

            var entity = new Finding
            {
                Descripcion = request.Descripcion,
                Tipo = request.Tipo,
                Severidad = request.Severidad,
                FechaHallazgo = request.FechaHallazgo,
                AuditId = request.AuditId,
                Created = DateTime.UtcNow,
                CreatedBy = "system"
            };

            _context.Findings.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}