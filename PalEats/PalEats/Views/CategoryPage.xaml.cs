using PalEats.Models;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PalEats.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : FlyoutPage
    {
        public CategoryPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            flyout.listview.ItemSelected += OnSelectedItem;

        }
        private async void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FlyoutMenuItem;
            var currentUser = ((App)App.Current).currentUser;
            if (currentUser == 0)
            {
                bool answer = await App.Current.MainPage.DisplayAlert("Shopping List", "You have to sign in first", "Yes", "No");
                if (answer)
                {
                    await App.Current.MainPage.Navigation.PushAsync(new SignInPage());
                }
            }
            if (item != null && currentUser > 0)
            {
                if (item.TargetPage == typeof(SignInPage))
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetPage)) { };
                }
                else
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetPage));
                }
                flyout.listview.SelectedItem = null;
                IsPresented = false;
            }
        }

        private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collectionView = sender as CollectionView;
            var selectedCategory = e.CurrentSelection.FirstOrDefault() as Category;

            if (selectedCategory != null)
            {
                collectionView.SelectedItem = null;

                string pageTitle =null;
                switch (selectedCategory.CategoryId)
                {
                    case 3:
                        pageTitle = "Sweets";
                        break;
                    case 2:
                        pageTitle = "Breakfast";
                        break;
                    case 4:
                        pageTitle = "Appetizers";
                        break;
                    case 1:
                        pageTitle = "Main Dish";
                        break;
                }

                await Navigation.PushAsync(new SubcategoryPage(pageTitle, selectedCategory.CategoryId));
            }
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}

