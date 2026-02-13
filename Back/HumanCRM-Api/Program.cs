using Microsoft.EntityFrameworkCore;
using HumanCRM_Api.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// ✅ Lê a connection string
var cs = builder.Configuration.GetConnectionString("DefaultConnection");

// ✅ Se vier como URL (postgres:// ou postgresql://), converte
if (!string.IsNullOrWhiteSpace(cs) &&
    (cs.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
     cs.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase)))
{
    cs = new NpgsqlConnectionStringBuilder(cs).ToString();
}

// ✅ Força SSL + TrustServerCertificate + desativa GSS (Railway/Linux)
var csb = new NpgsqlConnectionStringBuilder(cs)
{
    SslMode = SslMode.Require,
    TrustServerCertificate = true,
    GssEncryptionMode = GssEncryptionMode.Disable,
    Timeout = 30,
    CommandTimeout = 180,
    KeepAlive = 10
};

cs = csb.ToString();

// ✅ DbContext com retry (resolve transient failure)
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(cs, npgsql =>
    {
        npgsql.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null
        );
    })
);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("CorsPolicy");

// Static files
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
