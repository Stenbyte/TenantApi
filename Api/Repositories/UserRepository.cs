using TenantApi.Exceptions;
using TenantApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TenantApi.Repository;

public class UserRepository : IUserRepository
{
    private readonly TenantDbContext _dbContext;

    public UserRepository(TenantDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task Create(UserPg user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserPg?> FindUserByEmail(string? email)
    {
        if (email == null || email.Length == 0)
        {
            throw new CustomException("Email is required", null, 400);
        }

        return await _dbContext.Users.FirstOrDefaultAsync(u => u!.Email! == email);
    }

    public async Task<UserPg?> FindUserByRefreshToken(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            return null;
        }

        return await _dbContext.Users.FirstOrDefaultAsync(u => u.refreshToken == refreshToken);
    }

    public async Task UpdateUser(UserPg userToUpdate)
    {
        UserPg? user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userToUpdate.Id);

        if (user == null)
        {
            throw new CustomException("User not found", null, 400);
        }

        user.refreshToken = userToUpdate.refreshToken;
        user.refreshTokenExpiry = userToUpdate.refreshTokenExpiry;

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Guid?> GetBuildingIdForUser(Guid userId)
    {
        return await _dbContext.UserProperties
            .AsNoTracking()
            .Where(up => up.UserId == userId)
            .Select(up => (Guid?)up.Property.BuildingId)
            .FirstOrDefaultAsync();
    }
}
