using Domain.Entities;
using Domain.Enums;

namespace Audita360.Domain.Entities
{
    public class Tracking : BaseEntity
    {
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCompromiso { get; set; }
        public TrackingStatus Estado { get; set; } = TrackingStatus.Pendiente;

        public int FindingId { get; set; }
        public Finding Finding { get; set; } = null!;

        public bool CambiarEstado(TrackingStatus nuevoEstado)
        {
            if ((Estado == TrackingStatus.Pendiente && nuevoEstado == TrackingStatus.EnCurso) ||
                (Estado == TrackingStatus.EnCurso && nuevoEstado == TrackingStatus.Completado) ||
                (Estado == TrackingStatus.Pendiente && nuevoEstado == TrackingStatus.Completado) ||
                nuevoEstado == TrackingStatus.Pendiente) 
            {
                Estado = nuevoEstado;
                return true;
            }
            return false;
        }
    }
}