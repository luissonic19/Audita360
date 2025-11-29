namespace Audita360.Application.Reports.Queries.Models
{
    public class AreaDateSummaryDto
    {
        public string AreaAuditada { get; set; } = string.Empty;
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int TotalAuditorias { get; set; }
        public int TotalHallazgos { get; set; }
        public int HallazgosAlta { get; set; }
        public int HallazgosMedia { get; set; }
        public int HallazgosBaja { get; set; }
        public decimal PorcentajeSeguimientoCompletado { get; set; }
        public int AuditoriasConHallazgosAlta { get; set; }
    }
}