using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LivroApp.ViewModels.ShopVModels
{
    public class SelectLocationAndPaymentViewModel : BaseViewModel
    {
        public List<string> PaymentMethods { get; set; }
        public List<string> Locations { get; set; }
        public Command NextStep { get; }
        public int SelLocation;
        public string SelMethod;

        public SelectLocationAndPaymentViewModel()
        {
            Title = "Locatie si modalitate plata";
            PaymentMethods = DataStore.GetPaymentMtds();
            Locations = new List<string>();
            foreach (var loc in App.UserInfo.Locations)
                Locations.Add($"{App.UserInfo.Locations.IndexOf(loc) + 1} - {loc.LocationName}, {loc.BuildingInfo}, {loc.Street}, {loc.City}");
            IsBusy = false;

        }
    }
}
