using WebOptimize.Domain.Entities;

namespace WebOptimize.Application.DTOs
{
    public class OptimizationRequest
    {
        public List<Warehouse> Warehouses { get; set; } = new();
        public List<PickupPoint> PickupPoints { get; set; } = new();
        public string Objective { get; set; } = "MinimizeDistance"; // MinimizeDistance, MinimizeCost
    }
}
