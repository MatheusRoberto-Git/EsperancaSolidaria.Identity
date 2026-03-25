using EsperancaSolidaria.Identity.Domain.Entities;
using EsperancaSolidaria.Identity.Domain.Security.Tokens;
using EsperancaSolidaria.Identity.Domain.Services.LoggedUser;
using EsperancaSolidaria.Identity.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EsperancaSolidaria.Identity.Infrastructure.Services.LoggedUser
{
    public class LoggedUser : ILoggedUser
    {
        private readonly EsperancaSolidariaIdentityDbContext _dbContext;
        private readonly ITokenProvider _tokenProvider;

        public LoggedUser(EsperancaSolidariaIdentityDbContext dbContext, ITokenProvider tokenProvider)
        {
            _dbContext = dbContext;
            _tokenProvider = tokenProvider;
        }

        public async Task<User> User()
        {
            var token = _tokenProvider.Value();
            var tokenHanlder = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHanlder.ReadJwtToken(token);
            var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
            var userIdentifier = Guid.Parse(identifier);

            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
        }
    }
}