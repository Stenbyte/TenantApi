using TenantApi.Models;
using TenantApi.Repository;

namespace TenantApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Create(UserPg user)
        {
            await _repository.Create(user);
        }

        public async Task<UserPg?> FindUserByEmail(string email)
        {
            return await _repository.FindUserByEmail(email);
        }

        public async Task<UserPg?> FindUserByRefreshToken(string refreshToken)
        {
            return await _repository.FindUserByRefreshToken(refreshToken);
        }

        public async Task UpdateUser(UserPg userToUpdate)
        {
            await _repository.UpdateUser(userToUpdate);
        }
    }
}
