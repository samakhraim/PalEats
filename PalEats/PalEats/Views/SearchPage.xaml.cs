using PalEats.Models;
using PalEats.Views;
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
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }
        private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collectionView = sender as CollectionView;
            var selectedDish = e.CurrentSelection.FirstOrDefault() as Dish;

            if (selectedDish != null)
            {
                collectionView.SelectedItem = null;
                await Navigation.PushAsync(new RecipePage(selectedDish.DishId));
            }
        }
      
    }
}