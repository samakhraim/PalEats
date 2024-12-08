using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.Generic;
using System.Text;
using PalEats.Models;
using PalEats.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
namespace PalEats.ViewModels
{

    public class SubcategoryPageViewModel : INotifyPropertyChanged
    {
        private readonly DishPageServices dishService;
      
       
        public SubcategoryPageViewModel(int categoryId) 
        {
            dishService = new DishPageServices();
            CategoryId = categoryId;
            CategoryName = "main dish";
            LoadDishesAsync(categoryId);
        }

  
  

        private int categoryId;

        public int CategoryId
        {
            get { return categoryId; }
            set
            {
                if (categoryId != value)
                {
                    categoryId = value;
                    OnPropertyChanged(nameof(CategoryId));
                }
            }
        }
        private string categoryName;

        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                if (categoryName != value)
                {
                    categoryName = value;
                    OnPropertyChanged(nameof(CategoryName));
                }
            }
        }
      

        private ObservableCollection<Dish> dishes = new ObservableCollection<Dish>();

        public ObservableCollection<Dish> Dishes
        {
            get { return dishes; }
            set
            {
                dishes = value;
                OnPropertyChanged(nameof(Dishes));
            }
        }

        public async Task LoadDishesAsync(int categoryId)
        {
            try
            {
                List<Dish> dishesList = await dishService.GetDishesByCategoryId(categoryId);
                Dishes = new ObservableCollection<Dish>(dishesList);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while loading dishes: {ex.Message}");

                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while loading dishes. Please try again later.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
