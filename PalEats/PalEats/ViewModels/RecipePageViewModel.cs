using PalEats.Models;
using PalEats.Services;
using PalEats.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace PalEats.ViewModels
{
    public class RecipePageViewModel : INotifyPropertyChanged
    {
        private readonly RecipeServices recipeServices;

        public RecipePageViewModel(int selectedDish)
        {
            recipeServices = new RecipeServices();
            DishId = selectedDish;
            Task.Run(() => this.LoadRecipesAsync()).Wait();
            Task.Run(() => this.LoadIngredientsAsync()).Wait();
            Task.Run(() => this.LoadFavoriteStatusAsync()).Wait();
            if (Selected)
                FavoriteButtonClicked = new Command(async () => await RemoveFavoriteAsync());
            else
                FavoriteButtonClicked = new Command(async () => await AddToFavoriteAsync());




        }


        private bool Selected = false;

        public string FavoritePath
        {

            get
            {
                if (Selected)
                    return "favorite_selected.png";
                else
                    return "favorite.png";
            }
            set
            {
                ;
            }
        }
        private int selectedDish;

        public int DishId
        {
            get { return selectedDish; }
            set
            {
                if (selectedDish != value)
                {
                    selectedDish = value;
                    OnPropertyChanged(nameof(DishId));
                }
            }
        }
        public List<String> Preparation
        {
            get
            {
                String[] prep = Recipe.Preparation.Split('.');
                List<String> result = new List<String>();
                for (int i = 0; i < prep.Length - 1; i++)
                {
                    result.Add((i + 1) + ". " + prep[i]);
                }

                return result;
            }
            set {; }
        }
        public string NumberOfPeople
        {
            get { return "Serves " + Recipe.NumberOfPeople; }
            set {; }
        }
        public ICommand FavoriteButtonClicked { get; private set; }

        public int IngredientsHeight
        {
            get
            {
                return Ingredients.Count * 27;
                ;
            }
            set {; }
        }
        private Recipe recipe = new Recipe();
        public Recipe Recipe
        {
            get { return recipe; }
            set
            {
                recipe = value;
                OnPropertyChanged(nameof(Recipe));
            }
        }
        public async Task LoadRecipesAsync()
        {
            try
            {
                Recipe = await recipeServices.GetRecipesAsync(DishId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while loading recipes: {ex.Message}");

                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while loading recipes. Please try again later.", "OK");
            }
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
        public async Task LoadIngredientsAsync()
        {
            try
            {
                Ingredients = await recipeServices.GetIngredientsAsync(DishId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while loading recipes: {ex.Message}");

                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while loading recipes. Please try again later.", "OK");
            }
        }

        private async Task AddToFavoriteAsync()
        {
            try
            {
                var currentUser = ((App)App.Current).currentUser;

                if (currentUser == 0)
                {
                    bool answer = await App.Current.MainPage.DisplayAlert("Add To Favorite", "You have to sign in first to add this recipe to your favorites. Do you want to sign in?", "Yes", "No");
                    if (answer)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new SignInPage());
                    }
                }
                else
                {
                    var favoriteService = new FavoriteServices();

                    var result = await favoriteService.AddFavoriteAsync(currentUser, Recipe.DishId);

                    if (result > 0)
                    {
                        await App.Current.MainPage.Navigation.PushAsync(new RecipePage(DishId));
                    }
                    else if (result == 0)
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "You have already added this recipe to your favorites.", "OK");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Failed to add to favorites.", "OK");
                    }
                }
            }
            catch (ArgumentException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while adding to favorites.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadFavoriteStatusAsync()
        {
            try
            {
                var favoriteService = new FavoriteServices();
                Selected = await favoriteService.IsSelectedAsync(((App)App.Current).currentUser, DishId);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while loading recipes: {ex.Message}");

                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while loading recipes. Please try again later.", "OK");
            }
        }

        public async Task RemoveFavoriteAsync()
        {
            try
            {
                var favoriteService = new FavoriteServices();
                await favoriteService.RemoveFavoriteAsync(((App)App.Current).currentUser, DishId);

                await App.Current.MainPage.Navigation.PushAsync(new RecipePage(DishId));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while removing favorite: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while removing the favorite. Please try again later.", "OK");
            }
        }
    }
}