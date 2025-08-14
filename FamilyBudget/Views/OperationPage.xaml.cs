using FamilyBudget.Services;
using FamilyBudget.Data;
namespace FamilyBudget;

public partial class OperationPage : ContentPage
{
	public OperationPage()
	{
		InitializeComponent();
		var data = new Database();
		var tar = new TransactionService(data);
        BindingContext = new OperationViewModel(tar);
    }
}