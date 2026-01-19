using Microsoft.EntityFrameworkCore;
using HumanCRM_Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseRouting();

// 🔴 ESSENCIAL
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

// 🔴 Angular SPA fallback (TEM que ser o último)
app.MapFallbackToFile("/index.html");

app.Run();
