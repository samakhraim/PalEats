using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

using PalEats.Models;
using PalEats.Services;
using PalEats.Views;
using Xamarin.Forms;

namespace PalEats.ViewModel
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        private SignInServices signInService;
        public ICommand GuestClicked { get; }
        public ICommand SignInCommand { get; }
        public ICommand OnSignUpTapped{ get;  }

        public SignInViewModel()
        {
            SignInCommand = new Command(async () => await SignInAsync());
            OnSignUpTapped = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new SignUpPage()));
            signInService = new SignInServices();
            GuestClicked = new Command(async () => await OnGuestClicked());

        }

        private async Task OnGuestClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CategoryPage());
        }
        private SignUpModel signInModel = new SignUpModel();
        public SignUpModel SignInModel
        {
            get { return signInModel; }
            set
            {
                signInModel = value;
                OnPropertyChanged();
            }
        }



        private async Task SignInAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(SignInModel.Email))
                {
                    throw new ArgumentException("Please enter an email");
                }

                if (SignInModel.Email.IndexOf("@") == -1)
                {
                    throw new ArgumentException("Please enter a valid email");
                }

                if (string.IsNullOrEmpty(SignInModel.Password))
                {
                    throw new ArgumentException("Please enter a password");
                }

                var signUpService = new SignUpServices();

                var result = await signInService.AuthenticateUserAsync(SignInModel);

                if (result > 0)
                {
                    ((App)App.Current).currentUser = result;
                    ((App)App.Current).CurrentUserEmail = SignInModel.Email; 
                    App.NotifyLoginStatusUpdated(); 
                    await Application.Current.MainPage.Navigation.PushAsync(new CategoryPage());
                }


                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Incorrect email or password", "OK");
                }
            }
            catch (ArgumentException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Wrong Password ", "OK");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
