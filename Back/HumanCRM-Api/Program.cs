using Microsoft.EntityFrameworkCore;
using HumanCRM_Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin(); // 👈 produção (Railway)
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CorsPolicy");

// ⚠️ IMPORTANTE: Railway já é HTTPS
// app.UseHttpsRedirection(); ❌ REMOVER

// 👇 ESSENCIAL PARA O ANGULAR
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// 👇 SEMPRE POR ÚLTIMO
app.MapFallbackToFile("index.html");

app.Run();
