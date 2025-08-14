using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilyBudget.Models;
using FamilyBudget.Services;
using Microcharts;
using SkiaSharp;


namespace FamilyBudget.ViewModels;
public partial class StatisticViewModel : ObservableObject
{
    private readonly TransactionService _transactionService;
    private readonly UserModel _user;

    [ObservableProperty]
    private float balance;

    [ObservableProperty]
    private float totalExpenses;

    [ObservableProperty]
    private float totalIncome;

    [ObservableProperty]
    private Chart expenseChart;

    [ObservableProperty]
    private Chart incomeChart;

    [ObservableProperty]
    private Chart dailyExpenseChart;

    [ObservableProperty]
    private Chart dailyIncomeChart;

    [ObservableProperty]
    private Chart topExpenseCategoriesChart;

    public StatisticViewModel(TransactionService transactionService, UserModel user)
    {
        _transactionService = transactionService;
        _user = user;

        _user.Id = Preferences.Get("id", int.MinValue);

        _ = LoadStatisticsAsync();
    }

    private async Task LoadStatisticsAsync()
    {
        if (_user.Id == int.MinValue)
            return;

        var expenses = await _transactionService.GetExpensesAsync(_user.Id);
        var incomes = await _transactionService.GetIncomesAsync(_user.Id);

        TotalExpenses = (float)expenses.Sum(e => e.Amount);
        TotalIncome = (float)incomes.Sum(i => i.Amount);
        Balance = TotalIncome - TotalExpenses;

        var expenseGroups = expenses.GroupBy(e => e.Category)
                                    .Select(g => new { Category = g.Key, Amount = g.Sum(x => x.Amount) })
                                    .ToList();

        var incomeGroups = incomes.GroupBy(i => i.Category)
                                  .Select(g => new { Category = g.Key, Amount = g.Sum(x => x.Amount) })
                                  .ToList();

        ExpenseChart = new PieChart
        {
            Entries = CreateChartEntries(expenseGroups),
            LabelTextSize = 30,
            BackgroundColor = SKColors.Transparent
        };

        IncomeChart = new PieChart
        {
            Entries = CreateChartEntries(incomeGroups),
            LabelTextSize = 30,
            BackgroundColor = SKColors.Transparent
        };

        var dailyExpenses = expenses
    .GroupBy(e => e.Date.Date)
    .Select(g => new { Date = g.Key, Amount = g.Sum(x => x.Amount) })
    .OrderBy(g => g.Date)
    .ToList();

        DailyExpenseChart = new LineChart
        {
            Entries = CreateChartEntries(dailyExpenses.Select(d => new { Category = d.Date.ToString("dd.MM"), d.Amount })),
            LabelTextSize = 25,
            BackgroundColor = SKColors.Transparent
        };

        // Графік щоденного доходу
        var dailyIncomes = incomes
            .GroupBy(i => i.Date.Date)
            .Select(g => new { Date = g.Key, Amount = g.Sum(x => x.Amount) })
            .OrderBy(g => g.Date)
            .ToList();

        DailyIncomeChart = new LineChart
        {
            Entries = CreateChartEntries(dailyIncomes.Select(d => new { Category = d.Date.ToString("dd.MM"), d.Amount })),
            LabelTextSize = 25,
            BackgroundColor = SKColors.Transparent
        };

        // Графік топ-3 категорій витрат
        var topExpenses = expenseGroups.OrderByDescending(e => e.Amount).Take(3).ToList();

        TopExpenseCategoriesChart = new PieChart
        {
            Entries = CreateChartEntries(topExpenses),
            LabelTextSize = 30,
            BackgroundColor = SKColors.Transparent
        };
    }

    private List<ChartEntry> CreateChartEntries(IEnumerable<dynamic> groups)
    {
        var colors = new List<SKColor>
        {
            SKColors.OrangeRed,
            SKColors.CornflowerBlue,
            SKColors.MediumSeaGreen,
            SKColors.Goldenrod,
            SKColors.MediumPurple,
            SKColors.Tomato,
            SKColors.SlateBlue,
            SKColors.SeaGreen,
            SKColors.Crimson,
            SKColors.DarkCyan
        };

        var entries = new List<ChartEntry>();
        int colorIndex = 0;

        foreach (var group in groups)
        {
            var entry = new ChartEntry((float)group.Amount)
            {
                Label = group.Category,
                ValueLabel = group.Amount.ToString("F2"),
                Color = colors[colorIndex % colors.Count]
            };

            entries.Add(entry);
            colorIndex++;
        }

        if (!entries.Any())
        {
            entries.Add(new ChartEntry(1)
            {
                Label = "Немає даних",
                ValueLabel = "0",
                Color = SKColors.Gray
            });
        }

        return entries;
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadStatisticsAsync();
    }
}
