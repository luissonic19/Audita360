using Domain.Entities;

namespace Audita360.Domain.Entities
{
    public class Responsible : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;

        public ICollection<Audit> Audits { get; set; } = new List<Audit>();
    }
}
