using PalEats.Models;
using PalEats.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace PalEats.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritePage : ContentPage
    {
        FavoritePageViewModel viewModel;

        public FavoritePage()
        {
            InitializeComponent();
            viewModel = new FavoritePageViewModel();
            BindingContext = viewModel; 
        }
       

        private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
            {
                return;
            }

            Dish selectedDish = e.CurrentSelection.FirstOrDefault() as Dish;

            if (selectedDish != null)
            {
                await Navigation.PushAsync(new RecipePage(selectedDish.DishId));
            }

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}




