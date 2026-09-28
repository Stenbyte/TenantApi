using TenantApi.Exceptions;
using TenantApi.Models;

namespace TenantApi.Repository;

public class TenantRepository : ITenantRepository
{
    private readonly TenantDbContext _dbContext;

    public TenantRepository(TenantDbContext dBContext)
    {
        _dbContext = dBContext;
    }

    public string TestPgConnectionWithDbContext()
    {
        try
        {
            var canConnect = _dbContext.Database.CanConnect();
            return canConnect
                ? "✅ Successfully connected to Postgres via DbContext"
                : "❌ Failed to connect to Postgres via DbContext";
        }
        catch (Exception ex)
        {
            throw new CustomException("🍉🍉🍉 Failed to connect to Postgres with DbContext 🍉🍉🍉", ex, 500);
        }
    }
}
