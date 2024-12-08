using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using PalEats.Models;
using PalEats.Services;
using PalEats.Views;
using System.Text.RegularExpressions;


namespace PalEats.ViewModels
{
    public class SignUpPageViewModel : INotifyPropertyChanged
    {
        public SignUpPageViewModel()
        {
            SignUpButtonClicked = new Command(async () => await SignUpAsync());
        }

        private SignUpModel _signUpModel = new SignUpModel();
        public SignUpModel SignUpModel
        {
            get { return _signUpModel; }
            set
            {
                _signUpModel = value;
                OnPropertyChanged();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand SignUpButtonClicked { get; private set; }

        private async Task SignUpAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(SignUpModel.Email))
                {
                    throw new ArgumentException("Please enter an email");
                }

                string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

                bool isEmailValid = Regex.IsMatch(SignUpModel.Email, emailPattern);

                if (!isEmailValid)
                {
                    throw new ArgumentException("Please enter a valid email");
                }

                if (string.IsNullOrEmpty(SignUpModel.Password))
                {
                    throw new ArgumentException("Please enter a password");
                }

                if (SignUpModel.Password.Length < 8)
                {
                    throw new ArgumentException("Password must be at least 8 characters long");
                }

                if (SignUpModel.Password != ConfirmPassword)
                {
                    throw new ArgumentException("Passwords do not match");
                }

                var signUpService = new SignUpServices();

                var result = await signUpService.InsertNewUsersAsync(SignUpModel);

                if (result > 0)
                {
                    ((App)App.Current).currentUser = result;
                    ((App)App.Current).CurrentUserEmail = SignUpModel.Email; // Set the current user's email
                    App.NotifyLoginStatusUpdated(); // Notify that the login status has been updated
                    await Application.Current.MainPage.Navigation.PushAsync(new CategoryPage());
                }

                else if (result == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Email address is already in use", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Sign up failed", "OK");
                }
            }
            catch (ArgumentException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while signing up", "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
