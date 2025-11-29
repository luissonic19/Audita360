using Audita360.Application.Interfaces;
using MediatR;
using System;
using Microsoft.EntityFrameworkCore;

namespace Audita360.Application.Findings.Commands.DeleteFinding
{
    public class DeleteFindingCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteFindingCommandHandler : IRequestHandler<DeleteFindingCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteFindingCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteFindingCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Findings
                .Include(f => f.Audit) 
                .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new Exception("Hallazgo no encontrado");
            }

            if (!entity.PuedeEliminarse())
            {
                throw new Exception("No se puede eliminar el hallazgo. La auditoría debe estar en estado 'En Proceso'");
            }

            _context.Findings.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
