using PalEats.Models;
using PalEats.ViewModel;
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
    public partial class FlyoutMenuPage : ContentPage
    {
        public FlyoutMenuPage()
        {
            InitializeComponent();
            SetLoginStatus();
            App.LoginStatusUpdated += OnLoginStatusUpdated; // Subscribe to the login status updated event
            listview.ItemSelected += OnMenuItemSelected;

        }

        void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FlyoutMenuItem;

            if (item == null)
                return;

            if (item.Title == "Log Out")
            {
                ((App)App.Current).LogOut();
                // After logout, navigate to sign in page
                Navigation.PushAsync(new SignInPage());
            }
            else
            {
                // Navigate to the target page
                Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetPage));
            }

            // Deselect the item
            listview.SelectedItem = null;
        }

        private void SetLoginStatus()
        {
            var app = (App)Application.Current;
            if (app.currentUser > 0)
            {
                loginStatusLabel.Text = app.CurrentUserEmail;
            }
            else
            {
                loginStatusLabel.Text = " Guest";
            }
        }

        private void OnLoginStatusUpdated(object sender, EventArgs e)
        {
            SetLoginStatus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.LoginStatusUpdated -= OnLoginStatusUpdated; // Unsubscribe from the login status updated event
        }
    }
}
