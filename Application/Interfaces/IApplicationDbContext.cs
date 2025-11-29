using Audita360.Application.Reports.Queries.Models;
using Audita360.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace Audita360.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Audit> Audits { get; set; }
        DbSet<Finding> Findings { get; set; }
        DbSet<Responsible> Responsibles { get; set; }
        DbSet<Tracking> Trackings { get; set; }
        DbSet<AreaDateSummaryDto> AreaDateSummary { get; set; }
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
    }
}