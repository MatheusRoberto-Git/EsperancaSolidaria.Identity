using EsperancaSolidaria.Identity.Domain.Entities;
using EsperancaSolidaria.Identity.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace EsperancaSolidaria.Identity.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateRoleOnlyRepository
    {
        private readonly EsperancaSolidariaIdentityDbContext _dbContext;

        public UserRepository(EsperancaSolidariaIdentityDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

        public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);

        public async Task<bool> ExistActiveUserWithCPF(string cpf) => await _dbContext.Users.AnyAsync(user => user.CPF.Equals(cpf) && user.Active);

        public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await _dbContext.Users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);

        public async Task<User?> GetByEmail(string email)
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email));
        }

        public async Task<User> GetById(long id) => await _dbContext.Users.FirstAsync(user => user.Id == id);

        public void Update(User user) => _dbContext.Users.Update(user);
    }
}