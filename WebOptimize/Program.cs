using Microsoft.EntityFrameworkCore;
using WebOptimize.API.Endpoints;
using WebOptimize.Application.Interfaces;
using WebOptimize.Application.Services;
using WebOptimize.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Регистрация DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация сервисов
builder.Services.AddScoped<IOptimizationService, OptimizationService>();

var app = builder.Build();

// === Автоматическое создание базы данных ===
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    Console.WriteLine(" Ensuring database is created...");
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
    Console.WriteLine(" База данных пересоздана успешно!");
}
// ===========================================

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Minimal API endpoints
app.MapOptimizationEndpoints();

app.Run();