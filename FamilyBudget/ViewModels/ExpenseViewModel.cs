using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilyBudget.Models;
using FamilyBudget.Services;
using System.Collections.ObjectModel;

namespace FamilyBudget.ViewModels
{
    public partial class ExpenseViewModel : ObservableObject
    {
        private readonly UserModel _user;
        private readonly CategoryService _categoryService;
        private readonly TransactionService _transactionService;

        public ObservableCollection<CategoryModel> Categories { get; } = new();

        public ExpenseViewModel(CategoryService categoryService, UserModel user, TransactionService transactionService)
        {
            _categoryService = categoryService;
            _transactionService = transactionService;
            _user = user;

            _user.Id = Preferences.Get("id", int.MinValue);

            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            var cats = await _categoryService.GetCategoriesAsync(_user.Id, CategoryType.Expense);
            Categories.Clear();
            foreach (var c in cats)
                Categories.Add(c);
        }

        [RelayCommand]
        private async Task SelectCategoryAsync(CategoryModel category)
        {
            if (category.Id == -1)
            {
                string? result = await App.Current.MainPage.DisplayPromptAsync(
                    "Нова категорія",
                    "Введіть назву нової категорії",
                    "Додати",
                    "Скасувати");

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var newCategory = new CategoryModel
                    {
                        Title = result,
                        UserId = _user.Id,
                        IsDefault = false,
                        Type = CategoryType.Expense.ToString()
                    };

                    await _categoryService.AddCategoryAsync(newCategory);
                    await LoadCategoriesAsync();
                }
            }
            else
            {
                string result = await App.Current.MainPage.DisplayPromptAsync(
                    "Введіть суму",
                    $"Скільки витратили на {category.Title}?",
                    "OK",
                    "Скасувати",
                    "0",
                    keyboard: Keyboard.Numeric);

                if (!string.IsNullOrWhiteSpace(result) && decimal.TryParse(result, out decimal amount))
                {
                    var expense = new TransactionModel
                    {
                        Amount = amount,
                        Category = category.Title,
                        Date = DateTime.Now,
                        UserId = _user.Id
                    };

                    await _transactionService.SaveExpenseServiceAsync(expense);
                }
            }
        }
    }
}
