using LivroApp.ViewModels.ShopVModels;
using Xamarin.Forms;

namespace LivroApp.Views
{
    public partial class CategoryPage : ContentPage
    {
        readonly CategoryViewModel viewModel;
        public CategoryPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CategoryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadAllItems.Execute(null);

        }
    }
}