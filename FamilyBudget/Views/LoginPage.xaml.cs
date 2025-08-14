using FamilyBudget.ViewModels;
using FamilyBudget.Data;
using FamilyBudget.Services;

namespace FamilyBudget
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
			var database = new Database();
			var authservice = new AuthService(database);
            BindingContext = new LoginViewModel(authservice);
            LoadSavedCredentials();
        }

        private void LoadSavedCredentials()
        {
            var savedEmail = Preferences.Get("email", string.Empty);
            var savedPassword = Preferences.Get("password", string.Empty);

            var viewModel = BindingContext as LoginViewModel;
            if (viewModel != null)
            {
                viewModel.Email = savedEmail;
                viewModel.Password = savedPassword;
            }
        }
    }
}