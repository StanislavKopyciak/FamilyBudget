using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilyBudget.Models;
using FamilyBudget.Services;
using System.Collections.ObjectModel;

namespace FamilyBudget.ViewModels
{
    public partial class IncomeViewModel : ObservableObject
    {
        private readonly UserModel _user;
        private readonly CategoryService _categoryService;
        private readonly TransactionService _transactionService;

        [ObservableProperty]
        private ObservableCollection<CategoryModel> categories = new();

        public IncomeViewModel(CategoryService categoryService, UserModel user, TransactionService transactionService)
        {
            _categoryService = categoryService;
            _user = user;
            _transactionService = transactionService;
            _user.Id = Preferences.Get("id", int.MinValue);

            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            var cats = await _categoryService.GetCategoriesAsync(_user.Id, CategoryType.Income);
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
                        Type = CategoryType.Income.ToString()
                    };

                    await _categoryService.AddCategoryAsync(newCategory);
                    await LoadCategoriesAsync();
                }
            }
            else
            {
                string result = await App.Current.MainPage.DisplayPromptAsync(
                    "Введіть суму",
                    $"Скільки отримали на {category.Title}?",
                    "OK",
                    "Скасувати",
                    "0",
                    keyboard: Keyboard.Numeric);

                if (decimal.TryParse(result, out decimal amount))
                {
                    var income = new TransactionModel
                    {
                        Amount = amount,
                        Category = category.Title,
                        Date = DateTime.Now,
                        UserId = _user.Id
                    };

                    await _transactionService.SaveIncomeServiceAsync(income);
                }
            }
        }
    }
}
