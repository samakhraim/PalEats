using Microsoft.IdentityModel.Tokens;
using PalEats.Models;
using PalEats.Services;
using PalEats.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PalEats.ViewModels
{
    public class FavoritePageViewModel : INotifyPropertyChanged
    {
        private readonly FavoritePageServices favoritePageServices;

        public FavoritePageViewModel()
        {
            favoritePageServices = new FavoritePageServices();
            Task.Run(() => this.LoadFavoritePageAsync()).Wait();

        }
        private List<Dish> dish = new List<Dish>();
        public List<Dish> FavoriteDishes
        {
            get { return dish; }
            set
            {
                dish = value;
                OnPropertyChanged(nameof(Dish));
            }
        }
        public async Task LoadFavoritePageAsync()
        {
            Debug.WriteLine("labalan");
            try
            {
                FavoriteDishes = await favoritePageServices.GetFavoritepageAsync();
                if (FavoriteDishes.IsNullOrEmpty()) {
                    HasNoFav = true;
                }
                else
                {
                    HasNoFav = false;
                }
                HasFav = !HasNoFav;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while loading recipes: {ex.Message}");
                Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");

                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while loading recipes. Please try again later.", "OK");
            }
        }
        public bool HasNoFav { get; set; }
        public bool HasFav { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
