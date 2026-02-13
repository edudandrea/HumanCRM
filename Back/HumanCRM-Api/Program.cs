using Microsoft.EntityFrameworkCore;
using HumanCRM_Api.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Monta connection string interna do Railway
var cs =
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    $"Host={builder.Configuration["PGHOST"]};" +
    $"Port={builder.Configuration["PGPORT"]};" +
    $"Database={builder.Configuration["PGDATABASE"]};" +
    $"Username={builder.Configuration["PGUSER"]};" +
    $"Password={builder.Configuration["PGPASSWORD"]};" +
    "SSL Mode=Disable;";

// DbContext com retry (ok manter)
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
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
