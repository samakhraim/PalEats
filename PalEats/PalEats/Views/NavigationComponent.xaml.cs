using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PalEats.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationComponent : ContentView
    {
        public NavigationComponent()
        {
            InitializeComponent();
        }

        private async void Button_search_Clicked(object sender, EventArgs e)
        {
            var currentPage = Navigation.NavigationStack.LastOrDefault();
            if (!(currentPage is SearchPage))
            {
                await Navigation.PushModalAsync(new NavigationPage(new SearchPage()));
            }
        }


        private async void Button_Home_Clicked(object sender, EventArgs e)
        {
            var currentPage = Navigation.NavigationStack.LastOrDefault();
            if (!(currentPage is CategoryPage))
            {
                var categoryPage = new CategoryPage();

              
                Navigation.InsertPageBefore(categoryPage, Navigation.NavigationStack.First());

                await Navigation.PopToRootAsync();
            }
        }

        private async void Button_favorite_Clicked(object sender, EventArgs e)
        {
            var currentPage = Navigation.NavigationStack.LastOrDefault();
            if (!(currentPage is FavoritePage))
            {
                if (((App)Application.Current).currentUser == 0)
                {
                    bool answer = await Application.Current.MainPage.DisplayAlert("Favorite Page", "You are currently a guest. Do you want to sign in?", "Yes", "No");

                    if (answer)
                    {
                        await Navigation.PushAsync(new SignInPage());
                    }
                }
                else
                {
                    await Navigation.PushModalAsync(new NavigationPage(new FavoritePage()));
                }
            }
        }


    }
}
