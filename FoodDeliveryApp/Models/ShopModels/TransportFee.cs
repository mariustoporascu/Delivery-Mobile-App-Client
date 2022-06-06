using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Models.ShopModels
{
    public class TransportFee
    {
        public int CompanieRefId { get; set; }
        public int CityRefId { get; set; }
        public decimal TransporFee { get; set; }
        public decimal MinimumOrderValue { get; set; }
    }
}
