using PalEats.Models;
using PalEats.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PalEats.ViewModels
{
    public class CategoryPageViewModel : INotifyPropertyChanged
    {
        private readonly CategoryPageServices categoryServices;

        public CategoryPageViewModel()
        {
            categoryServices = new CategoryPageServices();
            LoadCategoriesAsync();
        }
        private List<Category> categories = new List<Category>();

        public List<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public async Task LoadCategoriesAsync()
        {
            try
            {
                Categories = await categoryServices.GetCategoriesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while loading categories: {ex.Message}");

                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while loading categories. Please try again later.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}