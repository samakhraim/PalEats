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
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void OnSignInTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignInPage());
        }
        private async void OnGuestButtonClicked(object sender, EventArgs e)
        {
            ((App)App.Current).currentUser = 0;
            await Navigation.PushAsync(new CategoryPage());
        }
    }
}