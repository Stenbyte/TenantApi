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

    public Task CreateUser(User user)
        => throw new CustomException("Mongo user store removed; use Postgres UserPg", null, 501);

    public async Task Create(UserPg user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public Task<User> FindUserById(string userId)
        => throw new CustomException("Mongo user store removed; use Postgres UserPg", null, 501);

    public Task<User> FindExistingUserWithDbName(User newUser)
        => throw new CustomException("Mongo user store removed; use Postgres UserPg", null, 501);

    public Task<User> FindUserByRefreshToken(string refreshToken)
        => throw new CustomException("Mongo user store removed; use Postgres UserPg", null, 501);

    public async Task<UserPg> FindUserByEmail(string? email)
    {
        if (email == null || email.Length == 0)
        {
            throw new CustomException("Email is required", null, 400);
        }

        UserPg? existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u!.Email! == email);

        if (existingUser is null)
        {
            throw new CustomException("User not found", null, 404);
        }
        return existingUser!;
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
}
