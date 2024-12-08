using PalEats.Models;
using PalEats.ViewModels;
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
    public partial class ShoppingListModal : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ShoppingListModal(List<Ingredients> ingredients)
        {
            InitializeComponent();
            var viewModel = new ShoppingListPageViewModel(ingredients);
            BindingContext = viewModel;
        }

        private void CloseButtonClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}