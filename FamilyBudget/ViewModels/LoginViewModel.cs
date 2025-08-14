using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilyBudget.Services;
using Microsoft.Maui.ApplicationModel.Communication;

namespace FamilyBudget.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string message = string.Empty;

        [RelayCommand]
        private async Task Login()
        {
            Message = await _authService.LogineAsynce(Email, Password);
            if (Message == "Ви успішно увійшли.")
            {
                await Shell.Current.GoToAsync("///ExpensePage");
                Email = string.Empty;
                Password = string.Empty;
            }
        }

        [RelayCommand]
        private async Task RegistrationLink()
        {
            await Shell.Current.GoToAsync("///RegistrationPage");
        }
    }
}

