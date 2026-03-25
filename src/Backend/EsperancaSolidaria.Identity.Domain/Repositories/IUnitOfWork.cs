namespace EsperancaSolidaria.Identity.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public Task Commit();
    }
}