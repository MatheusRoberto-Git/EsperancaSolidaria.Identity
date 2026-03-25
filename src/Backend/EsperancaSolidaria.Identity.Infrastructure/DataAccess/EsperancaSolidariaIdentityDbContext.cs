using EsperancaSolidaria.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EsperancaSolidaria.Identity.Infrastructure.DataAccess
{
    public class EsperancaSolidariaIdentityDbContext : DbContext
    {
        public EsperancaSolidariaIdentityDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EsperancaSolidariaIdentityDbContext).Assembly);
        }
    }
}