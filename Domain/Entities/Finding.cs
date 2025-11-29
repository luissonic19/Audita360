using Domain.Entities;
using Domain.Enums;

namespace Audita360.Domain.Entities
{
    public class Finding : BaseEntity
    {
        public string Descripcion { get; set; } = string.Empty;
        public FindingType Tipo { get; set; }
        public FindingSeverity Severidad { get; set; }
        public DateTime FechaHallazgo { get; set; }

        public int AuditId { get; set; }
        public Audit Audit { get; set; } = null!;

        public ICollection<Tracking> Trackings { get; set; } = new List<Tracking>();

        public bool PuedeEliminarse()
        {
            return Audit?.Estado == AuditStatus.EnProceso;
        }

        public bool TieneSeguimientosCompletados()
        {
            return Trackings.Any(t => t.Estado == TrackingStatus.Completado);
        }
    }
}