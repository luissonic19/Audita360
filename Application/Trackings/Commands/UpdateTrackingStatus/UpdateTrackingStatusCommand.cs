using Audita360.Application.Interfaces;
using Domain.Enums;
using MediatR;

namespace Audita360.Application.Trackings.Commands.UpdateTrackingStatus
{
    public class UpdateTrackingStatusCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public TrackingStatus NuevoEstado { get; set; }
    }

    public class UpdateTrackingStatusCommandHandler : IRequestHandler<UpdateTrackingStatusCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTrackingStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateTrackingStatusCommand request, CancellationToken cancellationToken)
        {
            var tracking = await _context.Trackings
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (tracking == null)
            {
                throw new Exception("Seguimiento no encontrado");
            }

            if (!tracking.CambiarEstado(request.NuevoEstado))
            {
                throw new Exception($"No se puede cambiar el estado de {tracking.Estado} a {request.NuevoEstado}");
            }

            tracking.LastModified = DateTime.UtcNow;
            tracking.LastModifiedBy = "system";

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}