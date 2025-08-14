using FamilyBudget.Models;
using FamilyBudget.Services;
using FamilyBudget.Data;
using FamilyBudget.ViewModels;

namespace FamilyBudget;

public partial class StatisticPage : ContentPage
{
	public StatisticPage()
	{
		InitializeComponent();
		var c = new Database();
		var a = new TransactionService(c);
		var b = new UserModel();
		BindingContext = new StatisticViewModel(a, b);
	}
}