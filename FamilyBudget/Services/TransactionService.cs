using FamilyBudget.Data;
using FamilyBudget.Models;
using static FamilyBudget.Models.CategoryModel;

namespace FamilyBudget.Services
{
    public class TransactionService
    {
        private readonly Database _db;

        public TransactionService(Database db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db), "Database dependency cannot be null.");
        }

        public async Task<string> SaveExpenseServiceAsync(TransactionModel model)
        {
            try
            {
                if (model == null)
                    return "Модель витрат не може бути null.";

                if (model.UserId <= 0)
                    return "Некоректний ID користувача.";

                if (string.IsNullOrWhiteSpace(model.Category))
                    return "Категорія витрат не може бути порожньою.";
                if (model.Category.Length > 50)
                    return "Назва категорії не може перевищувати 50 символів.";

                if (model.Amount <= 0)
                    return "Сума витрат повинна бути більшою за 0.";
                if (model.Amount > 1_000_000)
                    return "Сума витрат надто велика для збереження.";

                if (model.Date == default)
                    model.Date = DateTime.UtcNow;

                await _db.AddExpenseAsync(model);
                return "Витрата успішно збережена!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні витрат: {ex}");
                return "Сталася помилка при збереженні витрат.";
            }
        }

        public async Task<List<TransactionModel>> GetExpensesAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Некоректний ID користувача.", nameof(userId));

            try
            {
                var expenses = await _db.GetExpensesByUserAsync(userId);
                return expenses ?? new List<TransactionModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при отриманні витрат: {ex}");
                return new List<TransactionModel>();
            }
        }

        public async Task<string> SaveIncomeServiceAsync(TransactionModel model)
        {
            try
            {
                if (model == null)
                    return "Модель доходу не може бути null.";

                if (model.UserId <= 0)
                    return "Некоректний ID користувача.";

                if (string.IsNullOrWhiteSpace(model.Category))
                    return "Категорія доходу не може бути порожньою.";
                if (model.Category.Length > 50)
                    return "Назва категорії не може перевищувати 50 символів.";

                if (model.Amount <= 0)
                    return "Сума доходу повинна бути більшою за 0.";
                if (model.Amount > 1_000_000)
                    return "Сума доходу надто велика для збереження.";

                if (model.Date == default)
                    model.Date = DateTime.UtcNow;

                await _db.AddIncomeAsync(model);
                return "Дохід успішно збережений!";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні доходу: {ex}");
                return "Сталася помилка при збереженні доходу.";
            }
        }

        public async Task<List<TransactionModel>> GetIncomesAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("Некоректний ID користувача.", nameof(userId));

            try
            {
                var incomes = await _db.GetIncomeByUserAsync(userId);
                return incomes ?? new List<TransactionModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при отриманні доходів: {ex}");
                return new List<TransactionModel>();
            }
        }
    }

}
