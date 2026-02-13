using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace HumanCRM_Api.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            // 1) Preferencial: ConnectionStrings:DefaultConnection
            var cs = config.GetConnectionString("DefaultConnection");

            // 2) Alternativa: DATABASE_URL (Railway costuma ter)
            if (string.IsNullOrWhiteSpace(cs))
                cs = config["DATABASE_URL"];

            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException(
                    "Connection string não encontrada. Configure ConnectionStrings:DefaultConnection (appsettings/appsettings.Development) " +
                    "ou a variável ConnectionStrings__DefaultConnection/DATABASE_URL.");

            cs = cs.Trim().Trim('"'); // remove espaços e aspas acidentais

            // Se vier como URL (postgresql://...), converte para formato Npgsql
            if (cs.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
                cs.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
            {
                cs = new NpgsqlConnectionStringBuilder(cs).ToString();
            }

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql(cs)
                .Options;

            return new DataContext(options);
        }
    }
}
