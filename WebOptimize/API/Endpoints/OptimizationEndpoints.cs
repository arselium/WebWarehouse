using Microsoft.EntityFrameworkCore;
using WebOptimize.Application.DTOs;
using WebOptimize.Application.Interfaces;
using WebOptimize.Domain.Entities;
using WebOptimize.Infrastructure.Data;


namespace WebOptimize.API.Endpoints
{
    public static class OptimizationEndpoints
    {
        public static void MapOptimizationEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/optimize", async (OptimizationRequest req, IOptimizationService service) =>
            {
                var result = await service.OptimizeAsync(req);
                return Results.Ok(result);
            })
            .WithName("OptimizeDistribution");

            // ========================================
            app.MapGet("/api/warehouses", async (AppDbContext db) =>
                await db.Warehouses.ToListAsync())
            .WithName("GetAllWarehouses");

            app.MapGet("/api/warehouses/{id}", async (int id, AppDbContext db) =>
                await db.Warehouses.FindAsync(id) is Warehouse warehouse
                    ? Results.Ok(warehouse)
                    : Results.NotFound())
            .WithName("GetWarehouseById");

            app.MapPost("/api/warehouses", async (Warehouse warehouse, AppDbContext db) =>
            {
                db.Warehouses.Add(warehouse);
                await db.SaveChangesAsync();
                return Results.Created($"/api/warehouses/{warehouse.Id}", warehouse);
            })
            .WithName("CreateWarehouse");

            app.MapPut("/api/warehouses/{id}", async (int id, Warehouse updated, AppDbContext db) =>
            {
                var warehouse = await db.Warehouses.FindAsync(id);
                if (warehouse is null) return Results.NotFound();

                warehouse.Name = updated.Name;
                warehouse.Location = updated.Location;
                // warehouse.Stock = updated.Stock;

                await db.SaveChangesAsync();
                return Results.Ok(warehouse);
            })
            .WithName("UpdateWarehouse");

            app.MapDelete("/api/warehouses/{id}", async (int id, AppDbContext db) =>
            {
                var warehouse = await db.Warehouses.FindAsync(id);
                if (warehouse is null) return Results.NotFound();

                db.Warehouses.Remove(warehouse);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteWarehouse");

            // ========================================
            app.MapGet("/api/pickuppoints", async (AppDbContext db) =>
                await db.PickupPoints.ToListAsync())
            .WithName("GetAllPickupPoints");

            app.MapPost("/api/pickuppoints", async (PickupPoint point, AppDbContext db) =>
            {
                db.PickupPoints.Add(point);
                await db.SaveChangesAsync();
                return Results.Created($"/api/pickuppoints/{point.Id}", point);
            })
            .WithName("CreatePickupPoint");

        }
    }
}
