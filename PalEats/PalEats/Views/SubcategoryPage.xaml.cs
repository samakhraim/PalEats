using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PalEats.ViewModels;
using System.Globalization;
using System.Linq;
using PalEats.Services;
using PalEats.Models;

namespace PalEats.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubcategoryPage : ContentPage
    {
        SubcategoryPageViewModel viewModel;
        public SubcategoryPage(string categoryName, int categoryId)
        {
            InitializeComponent();
            viewModel = new SubcategoryPageViewModel(categoryId);
            viewModel.CategoryName = categoryName;
            BindingContext = viewModel;
        }

        private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             var collectionView = sender as CollectionView;
             var selectedDish = e.CurrentSelection.FirstOrDefault() as Dish;

             if (selectedDish != null)
             {
                 collectionView.SelectedItem = null;
                 var recipePage = new RecipePage(selectedDish.DishId);
                 recipePage.Title = selectedDish.DishName;
                 await Navigation.PushAsync(recipePage);

             }
         }
    }

}

