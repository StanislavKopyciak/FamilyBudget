using FamilyBudget.ViewModels;
using FamilyBudget.Data;
using FamilyBudget.Services;


namespace FamilyBudget
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();

            var database = new Database();
            var authservice = new AuthService(database);
            BindingContext = new RegistrationViewModel(authservice);
        }
    }
}