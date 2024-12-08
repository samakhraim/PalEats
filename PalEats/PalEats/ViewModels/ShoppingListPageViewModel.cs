using PalEats.Models;
using PalEats.Services;
using PalEats.Views;
using Prism.Navigation.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PalEats.ViewModels
{
    internal class ShoppingListPageViewModel : INotifyPropertyChanged
    {
        private readonly ShoppingListPageServices shoppingListPageServices = new ShoppingListPageServices();

        public ShoppingListPageViewModel(List<Ingredients> ingredients)
        {
            Ingredients = ingredients;
            foreach (var ingredient in Ingredients)
            {
                ingredient.IsChecked = true;
            }
            SaveCommand = new Command(async () => await SaveToShoppingCartAsync());
        }
        public ShoppingListPageViewModel()
        {
            LoadIngredientsAsync();
        }

        public Command SaveCommand { get; set; }

        public async Task LoadIngredientsAsync()
        {
            try
            {
                Ingredients = await shoppingListPageServices.getShoppingList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Loading Ingredients: " + ex.Message);
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to LoadIngredients. Message: " + ex.Message, "OK");
            }

        }

        private async Task SaveToShoppingCartAsync()
        {
            ShoppingListPageServices shoppingListPageServices = new ShoppingListPageServices();
            try
            {
                await shoppingListPageServices.AddToShoppingList(Ingredients);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error saving shopping list: " + ex.Message);
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to save shopping list. Message: " + ex.Message, "OK");
            }
            await Application.Current.MainPage.DisplayAlert("Success", "Ingredients saved!", "OK");

        }
        public int IngredientsHeight
        {
            get
            {
                return Ingredients.Count * 33;
                ;
            }
            set {; }
        }

        private List<Ingredients> ingredients = new List<Ingredients>();
        public List<Ingredients> Ingredients
        {
            get { return ingredients; }
            set
            {
                ingredients = value;
                OnPropertyChanged(nameof(Ingredients));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
