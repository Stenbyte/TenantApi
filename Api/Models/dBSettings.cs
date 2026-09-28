namespace TenantApi.Models
{
    public class PostgresSettings
    {
        public string Host { get; set; } = null!;

        public string Port { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
