using Domain.Entities;
using Domain.Enums;

namespace Audita360.Domain.Entities
{
    public class Audit : BaseEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string AreaAuditada { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public AuditStatus Estado { get; set; } = AuditStatus.Pendiente;

        public int ResponsibleId { get; set; }
        public Responsible Responsible { get; set; } = null!;

        public ICollection<Finding> Findings { get; set; } = new List<Finding>();

        public bool PuedeActualizarse()
        {
            return Estado == AuditStatus.Pendiente || Estado == AuditStatus.EnProceso;
        }

        public bool CambiarEstado(AuditStatus nuevoEstado)
        {
            if (PuedeActualizarse() || nuevoEstado == AuditStatus.Finalizada || nuevoEstado == AuditStatus.Cancelada)
            {
                Estado = nuevoEstado;
                return true;
            }
            return false;
        }
    }
}