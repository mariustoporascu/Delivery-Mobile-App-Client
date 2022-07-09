namespace LivroApp.Models.MapsModels
{
    public class StatsModel : BaseModel
    {
        private string _title = string.Empty;
        private string _distToGo = string.Empty;
        private string _timeToGo = string.Empty;
        public string Title { get { return _title; } set { SetProperty(ref _title, value); } }
        public string DistToGo { get { return _distToGo; } set { SetProperty(ref _distToGo, value); } }
        public string TimeToGo { get { return _timeToGo; } set { SetProperty(ref _timeToGo, value); } }
    }
}
