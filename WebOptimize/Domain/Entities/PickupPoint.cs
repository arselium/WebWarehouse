using WebOptimize.Domain.ValueObjects;

namespace WebOptimize.Domain.Entities
{
    public class PickupPoint
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Location Location { get; set; } = null!;
        public Dictionary<string, int> Demand { get; set; } = new();
    }
}
