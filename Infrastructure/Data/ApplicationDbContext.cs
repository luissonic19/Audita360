using Audita360.Application.Interfaces;
using Audita360.Application.Reports.Queries.Models;
using Audita360.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audita360.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Finding> Findings { get; set; }
        public DbSet<Responsible> Responsibles { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<AreaDateSummaryDto> AreaDateSummary { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Responsible
            modelBuilder.Entity<Responsible>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Area)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Nombre);
                entity.HasIndex(e => e.Correo).IsUnique(); // Correo único
                entity.HasIndex(e => e.Area);
            });

            modelBuilder.Entity<Audit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.AreaAuditada).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Estado).IsRequired().HasConversion<int>();
                entity.Property(e => e.FechaInicio).IsRequired();
                entity.Property(e => e.FechaFin).IsRequired();

                entity.HasOne(a => a.Responsible)
                    .WithMany(r => r.Audits)
                    .HasForeignKey(a => a.ResponsibleId)
                    .OnDelete(DeleteBehavior.Restrict); 

                entity.HasIndex(e => e.Estado);
                entity.HasIndex(e => e.AreaAuditada);
                entity.HasIndex(e => e.ResponsibleId); 
            });

            modelBuilder.Entity<Finding>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Tipo).IsRequired().HasConversion<int>();
                entity.Property(e => e.Severidad).IsRequired().HasConversion<int>();
                entity.Property(e => e.FechaHallazgo).IsRequired();

                entity.HasOne(f => f.Audit)
                    .WithMany(a => a.Findings)
                    .HasForeignKey(f => f.AuditId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.AuditId);
                entity.HasIndex(e => e.Tipo);
                entity.HasIndex(e => e.Severidad);
            });

            // Tracking
            modelBuilder.Entity<Tracking>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FechaCompromiso)
                    .IsRequired();

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasConversion<int>();

                entity.HasOne(t => t.Finding)
                    .WithMany(f => f.Trackings)
                    .HasForeignKey(t => t.FindingId)
                    .OnDelete(DeleteBehavior.Cascade); 

                entity.HasIndex(e => e.FindingId);
                entity.HasIndex(e => e.Estado);
                entity.HasIndex(e => e.FechaCompromiso);
            });

            modelBuilder.Entity<Tracking>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.FechaCompromiso)
                    .IsRequired();

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasConversion<int>();

                entity.HasOne(t => t.Finding)
                    .WithMany(f => f.Trackings)
                    .HasForeignKey(t => t.FindingId)
                    .OnDelete(DeleteBehavior.Cascade); 

                entity.HasIndex(e => e.FindingId);
                entity.HasIndex(e => e.Estado);
                entity.HasIndex(e => e.FechaCompromiso);
            });

            modelBuilder.Entity<AreaDateSummaryDto>()
            .HasNoKey()
            .ToView("vw_AuditoriasFinalizadasReporte");
        }

    }
}