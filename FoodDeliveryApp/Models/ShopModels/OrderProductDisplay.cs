namespace FoodDeliveryApp.Models.ShopModels
{
    public class OrderProductDisplay
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string GramajInterfata { get; set; }
        public string PretInterfata { get; set; }
        public int Cantitate { get; set; }
        public string ClientComments { get; set; }
        public bool HasComments { get; set; }
    }
}
