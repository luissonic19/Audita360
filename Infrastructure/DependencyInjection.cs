using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Audita360.Infrastructure.Data;
using Audita360.Domain.Entities;
using Audita360.Application.Interfaces; // ← Agregar este using
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Audita360.Infrastructure.Services;

namespace Audita360.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            // Identity
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // JWT Service
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}