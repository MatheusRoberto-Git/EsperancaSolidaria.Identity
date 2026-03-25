using EsperancaSolidaria.Identity.Domain.Entities;
using EsperancaSolidaria.Identity.Domain.Repositories.Token;
using Microsoft.EntityFrameworkCore;

namespace EsperancaSolidaria.Identity.Infrastructure.DataAccess.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly EsperancaSolidariaIdentityDbContext _dbContext;

        public TokenRepository(EsperancaSolidariaIdentityDbContext context) => _dbContext = context;

        public async Task<RefreshToken?> Get(string refreshToken)
        {
            return await _dbContext
                .RefreshTokens
                .AsNoTracking()
                .Include(token => token.User)
                .FirstOrDefaultAsync(token => token.Value.Equals(refreshToken));
        }

        public async Task SaveNewRefreshToken(RefreshToken refreshToken)
        {
            var tokens = _dbContext.RefreshTokens.Where(token => token.UserId == refreshToken.UserId);

            _dbContext.RefreshTokens.RemoveRange(tokens);

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
        }
    }
}