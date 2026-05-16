using WebOptimize.Domain.ValueObjects;


namespace WebOptimize.Domain.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Location Location { get; set; } = null!;
        public Dictionary<string, int> Stock { get; set; } = new(); // ProductId -> Quantity
    }
}
