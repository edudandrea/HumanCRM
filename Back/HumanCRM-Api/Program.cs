using Microsoft.EntityFrameworkCore;
using HumanCRM_Api.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();

    for (var i = 1; i <= 5; i++)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch
        {
            if (i == 5) throw;
            Thread.Sleep(3000);
        }
    }
}

// CORS
app.UseCors("CorsPolicy");

// Static files
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
