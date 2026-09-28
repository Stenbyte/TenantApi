using TenantApi.Models;

namespace TenantApi.Services;

public interface IUserService
{
    Task Create(UserPg user);
    Task<UserPg?> FindUserByEmail(string email);
    Task<UserPg?> FindUserByRefreshToken(string refreshToken);
    Task UpdateUser(UserPg userToUpdate);
}
