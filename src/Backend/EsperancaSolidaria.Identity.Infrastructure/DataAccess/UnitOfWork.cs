using EsperancaSolidaria.Identity.Domain.Repositories;

namespace EsperancaSolidaria.Identity.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EsperancaSolidariaIdentityDbContext _dbContext;

        public UnitOfWork(EsperancaSolidariaIdentityDbContext dbContext) => _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}