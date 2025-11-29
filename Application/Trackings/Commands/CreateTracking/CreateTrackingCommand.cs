using Audita360.Application.Interfaces;
using Audita360.Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Audita360.Application.Trackings.Commands.CreateTracking
{
    public class CreateTrackingCommand : IRequest<int>
    {
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCompromiso { get; set; }
        public int FindingId { get; set; } // Hallazgo asociado
    }

    public class CreateTrackingCommandHandler : IRequestHandler<CreateTrackingCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateTrackingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateTrackingCommand request, CancellationToken cancellationToken)
        {
            var finding = await _context.Findings
                .FindAsync(new object[] { request.FindingId }, cancellationToken);

            if (finding == null)
            {
                throw new Exception($"No se encontró un hallazgo con ID {request.FindingId}");
            }

            var entity = new Tracking
            {
                Descripcion = request.Descripcion,
                FechaCompromiso = request.FechaCompromiso,
                FindingId = request.FindingId,
                Estado = TrackingStatus.Pendiente,
                Created = DateTime.UtcNow,
                CreatedBy = "system"
            };

            _context.Trackings.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}