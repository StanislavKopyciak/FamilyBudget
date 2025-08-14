using FamilyBudget.ViewModels;
using FamilyBudget.Services;
using FamilyBudget.Data;
using FamilyBudget.Models;

namespace FamilyBudget
{
	public partial class ExpensePage : ContentPage
	{
		public ExpensePage()
		{
			InitializeComponent();
            var database = new Database();
            var tran = new TransactionService(database);
			var user = new UserModel();
			var cat = new CategoryService(database);
			BindingContext = new ExpenseViewModel(cat, user, tran);

		}
	}
}