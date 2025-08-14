
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FamilyBudget.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task Exit()
        {
            Preferences.Remove("id");
            Preferences.Remove("name");
            Preferences.Remove("email");
            Preferences.Remove("phone");
            Preferences.Remove("password");

            await Shell.Current.GoToAsync("///LoginPage");
        }
    }
}
