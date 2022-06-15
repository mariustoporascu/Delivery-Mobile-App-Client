using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryApp.Models.MapsModels
{
    public class StatsModel : BaseModel
    {
        private string _title;
        private string _distToGo;
        private string _timeToGo;
        public string Title { get { return _title; } set { SetProperty(ref _title, value); } }
        public string DistToGo { get { return _distToGo; } set { SetProperty(ref _distToGo, value); } }
        public string TimeToGo { get { return _timeToGo; } set { SetProperty(ref _timeToGo, value); } }
    }
}
