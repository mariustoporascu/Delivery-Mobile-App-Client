namespace LivroApp.Models.ShopModels
{
    public class TransportFee
    {
        public int CompanieRefId { get; set; }
        public int CityRefId { get; set; }
        public decimal TransporFee { get; set; }
        public decimal MinimumOrderValue { get; set; }
    }
}
