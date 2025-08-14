using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilyBudget.Models;
using FamilyBudget.Services;

namespace FamilyBudget.ViewModels
{
    public partial class RegistrationViewModel : ObservableObject
    {
        private readonly AuthService _authService;

        public RegistrationViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string phone = string.Empty;

        [ObservableProperty]
        private string message = string.Empty;

        [RelayCommand]
        public async Task Registration()
        {
            UserModel user = new UserModel
            {
                Name = name,
                Email = email,
                Password = password,
                Phone = phone
            };


            Message = await _authService.RegistrationAsync(user);
        }

        [RelayCommand]
        private async Task LoginLink()
        {
            await Shell.Current.GoToAsync("///LoginPage");
        }
    }
}
