namespace WebOptimize.Domain.Entities
{
    public class Shipment
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int PickupPointId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Distance { get; set; }
        public double Cost { get; set; }
    }
}
