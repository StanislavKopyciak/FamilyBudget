using FamilyBudget.Models;
using FamilyBudget.Data;

namespace FamilyBudget.Services
{
    public class CategoryService
    {
        private readonly Database _db;

        public CategoryService(Database db)
        {
            _db = db;
        }

        public async Task<List<CategoryModel>> GetCategoriesAsync(int userId, CategoryType type)
        {
            var defaultCategories = type == CategoryType.Expense ? CategoryModel.GetDefaultExpenseCategories() : CategoryModel.GetDefaultIncomeCategories();

            var dbCategories = await _db.GetCategoryByUserAsync(userId, type.ToString());

            if (dbCategories != null && dbCategories.Count > 0)
                defaultCategories.AddRange(dbCategories);

            defaultCategories.Add(new CategoryModel { Title = "+ Додати", Id = -1 });

            return defaultCategories;
        }

        public Task AddCategoryAsync(CategoryModel category)
        {
            return _db.AddCategoryAsync(category);
        }
    }
}
