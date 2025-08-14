using FamilyBudget.ViewModels;
using FamilyBudget.Services;
using FamilyBudget.Data;
using FamilyBudget.Models;

namespace FamilyBudget
{
    public partial class IncomePage : ContentPage
    {
        public IncomePage()
        {
            InitializeComponent();
            var database = new Database();
            var tran = new TransactionService(database);
            var user = new UserModel();
            var cat = new CategoryService(database);
            BindingContext = new IncomeViewModel(cat, user, tran);
        }
    }
}