using TenantApi.Exceptions;
using TenantApi.Repository;

namespace TenantApi.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _repository;

        public TenantService(ITenantRepository repository)
        {
            _repository = repository;
        }

        public string TestPgConnectionWithDbContext()
        {
            try
            {
                return _repository.TestPgConnectionWithDbContext();
            }
            catch (CustomException ex)
            {
                throw new CustomException("DataBase connection failed", ex, 500);
            }
        }
    }
}
