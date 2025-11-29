using Audita360.Application.Interfaces;
using Audita360.Domain.Entities;
using MediatR;

namespace Audita360.Application.Responsibles.Commands.CreateResponsible
{
    public class CreateResponsibleCommand : IRequest<int>
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
    }

    public class CreateResponsibleCommandHandler : IRequestHandler<CreateResponsibleCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateResponsibleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateResponsibleCommand request, CancellationToken cancellationToken)
        {
            var entity = new Responsible
            {
                Nombre = request.Nombre,
                Correo = request.Correo,
                Area = request.Area,
                Created = DateTime.UtcNow,
                CreatedBy = "system"
            };

            _context.Responsibles.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}