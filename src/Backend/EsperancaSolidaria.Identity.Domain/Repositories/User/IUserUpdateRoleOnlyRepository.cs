namespace EsperancaSolidaria.Identity.Domain.Repositories.User
{
    public interface IUserUpdateRoleOnlyRepository
    {
        public Task<Entities.User> GetById(long id);
        public void Update(Entities.User user);
    }
}