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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CorsPolicy");

// 🔴 ESSENCIAL (SEM ISSO NADA FUNCIONA)
app.UseRouting();

// 👇 ESSENCIAL PARA SERVIR O ANGULAR
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// API
app.MapControllers();

// Angular (SEMPRE POR ÚLTIMO)
app.MapFallbackToFile("index.html");

app.Run();
