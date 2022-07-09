namespace LivroApp.Models.ShopModels
{
    public class DriverLocation
    {
        public string Id { get; set; }
        public int OrderId { get; set; }
        public double CoordX { get; set; }
        public double CoordY { get; set; }
    }
}
