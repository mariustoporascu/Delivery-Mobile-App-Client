using FoodDeliveryApp.Constants;
using FoodDeliveryApp.Models.AuthModels;
using FoodDeliveryApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FoodDeliveryApp.ViewModels
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
            PaymentMethods = new List<string>();
            PaymentMethods.Add("Plata cash la livrare");
            PaymentMethods.Add("Plata cu cardul la livrare");
            Locations = new List<string>();
            foreach (var loc in App.UserInfo.Locations)
                Locations.Add($"{loc.LocationName},{loc.BuildingInfo},{loc.Street},{loc.City}");
        }

    }
}
