using TenantApi.Models;

namespace TenantApi.Repository;

public interface IUserRepository
{
    Task Create(UserPg user);
    Task<UserPg?> FindUserByEmail(string email);
    Task<UserPg?> FindUserByRefreshToken(string refreshToken);
    Task UpdateUser(UserPg userToUpdate);
}
