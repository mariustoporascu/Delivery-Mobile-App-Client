namespace LivroApp.Models.ShopModels
{
    public class ProductInOrder
    {
        public int OrderRefId { get; set; }

        public int ProductRefId { get; set; }

        public int UsedQuantity { get; set; }
        public string ClientComments { get; set; }
    }
}
