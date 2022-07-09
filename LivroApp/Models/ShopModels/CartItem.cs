namespace LivroApp.Models.ShopModels
{
    public class CartItem : BaseModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Gramaj { get; set; }

        private decimal _priceTotal;
        public decimal PriceTotal { get => _priceTotal; set => SetProperty(ref _priceTotal, value); }
        public int CompanieRefId { get; set; }

        private string _clientComments = string.Empty;
        public string ClientComments { get => _clientComments; set => SetProperty(ref _clientComments, value); }

        private int _cantitate;
        public int Cantitate { get => _cantitate; set => SetProperty(ref _cantitate, value); }
    }
}