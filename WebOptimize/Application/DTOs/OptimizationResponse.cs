using WebOptimize.Domain.Entities;

namespace WebOptimize.Application.DTOs
{
    public class OptimizationResponse
    {
        public List<Shipment> Shipments { get; set; } = new();
        public double TotalDistance { get; set; }
        public double TotalCost { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
