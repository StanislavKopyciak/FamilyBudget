using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilyBudget.Models;
using FamilyBudget.Services;
using System.Collections.ObjectModel;


namespace FamilyBudget
{
    public class OperationItem
    {
        public string? Type { get; set; } 
        public string? Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public partial class OperationViewModel : ObservableObject
    {
        [ObservableProperty]
        private TransactionModel selectedTransaction;

        public ObservableCollection<OperationItem> Operations { get; set; } = new ObservableCollection<OperationItem>();

        private readonly TransactionService _transactionService;
        private readonly UserModel _user;

        public OperationViewModel(TransactionService transactionService)
        {
            _transactionService = transactionService;
            _user = new UserModel();
            _user.Id = Preferences.Get("id", int.MinValue);

            LoadOperationsAsync();
        }

        [ObservableProperty]
        private decimal balance = 0;


        [RelayCommand]
        private async Task LoadOperationsAsync()
        {
            if (_user.Id == int.MinValue) return;

            var expenses = await _transactionService.GetExpensesAsync(_user.Id);
            var incomes = await _transactionService.GetIncomesAsync(_user.Id);

            var allOps = expenses.Select(e => new OperationItem
            {
                Type = "Витрата",
                Category = e.Category,
                Amount = e.Amount,
                Date = e.Date
            })
            .Concat(incomes.Select(i => new OperationItem
            {
                Type = "Доход",
                Category = i.Category,
                Amount = i.Amount,
                Date = i.Date
            }))

            .OrderByDescending(op => op.Date)
            .ToList();

            Operations.Clear();
            foreach (var op in allOps)
                Operations.Add(op);

            var totalIncome = incomes.Sum(i => i.Amount);
            var totalExpense = expenses.Sum(e => e.Amount);

            Balance = totalIncome - totalExpense;
        }
    }
}
