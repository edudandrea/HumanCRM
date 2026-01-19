using Microsoft.EntityFrameworkCore;
using HumanCRM_Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// CORS
app.UseCors("CorsPolicy");

// 🔴 1️⃣ SERVIR ARQUIVOS ESTÁTICOS PRIMEIRO
app.UseDefaultFiles();   // index.html
app.UseStaticFiles();    // js, css, assets

// 🔴 2️⃣ ROUTING
app.UseRouting();

// 🔴 3️⃣ AUTH (se houver)
app.UseAuthorization();

// 🔴 4️⃣ API
app.MapControllers();

// 🔴 5️⃣ SPA FALLBACK (TEM QUE SER O ÚLTIMO)
app.MapFallbackToFile("/index.html");

app.Run();
