namespace EsperancaSolidaria.Identity.Domain.Repositories.User
{
    public interface IUserWriteOnlyRepository
    {
        public Task Add(Entities.User user);
    }
}