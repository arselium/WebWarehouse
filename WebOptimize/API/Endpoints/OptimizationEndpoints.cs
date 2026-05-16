using WebOptimize.Application.DTOs;
using WebOptimize.Application.Interfaces;
using WebOptimize.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace WebOptimize.API.Endpoints
{
    public static class OptimizationEndpoints
    {
        public static void MapOptimizationEndpoints(this IEndpointRouteBuilder app)
        {
            // Основной эндпоинт оптимизации
            app.MapPost("/api/optimize", async (OptimizationRequest req, IOptimizationService service) =>
            {
                var result = await service.OptimizeAsync(req);
                return Results.Ok(result);
            })
            .WithName("OptimizeDistribution");

            // Получить все склады
            app.MapGet("/api/warehouses", async (AppDbContext db) =>
                await db.Warehouses.ToListAsync())
            .WithName("GetWarehouses");

            // Получить все пункты выдачи
            app.MapGet("/api/pickuppoints", async (AppDbContext db) =>
                await db.PickupPoints.ToListAsync())
            .WithName("GetPickupPoints");
        }
    }
}
