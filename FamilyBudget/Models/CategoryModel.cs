
namespace FamilyBudget.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int UserId { get; set; }
        public bool IsDefault { get; set; }
        public string? Type {  get; set; }

        public static List<CategoryModel> GetDefaultExpenseCategories()
        {
            return new List<CategoryModel>
            {
                new CategoryModel { Id = 1, Title = "Їжа", IsDefault = true, Type = "Expense"},
                new CategoryModel { Id = 2, Title = "Транспорт", IsDefault = true, Type = "Expense" },
                new CategoryModel { Id = 3, Title = "Розваги", IsDefault = true, Type = "Expense" },
                new CategoryModel { Id = 4, Title = "Здоровья", IsDefault = true, Type = "Expense"},
                new CategoryModel { Id = 5, Title = "Тварини", IsDefault = true, Type = "Expense" },
                new CategoryModel { Id = 6, Title = "Сім'я", IsDefault = true, Type = "Expense" },
                new CategoryModel { Id = 7, Title = "Одежда", IsDefault = true, Type = "Expense"},

            };
        }

        public static List<CategoryModel> GetDefaultIncomeCategories()
        {
            return new List<CategoryModel>
            {
                new CategoryModel { Id = 101, Title = "Зарплата", IsDefault = true, Type = "Income" }
            };
        }    
    }
}
