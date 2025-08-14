using FamilyBudget.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace FamilyBudget.Data
{
    public class Database
    {
        private readonly string connectionString;

        public Database(string connectionString = "server=localhost;user=root;password=root;database=familybudget")
        {
            this.connectionString = connectionString;
        }

        public async Task<UserModel?> GetUserAsync(string email, string password)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT id, name, email, password, phone FROM users WHERE email = @Email AND password = @Password LIMIT 1;", connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new UserModel
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Email = reader.GetString("email"),
                    Password = reader.GetString("password"),
                    Phone = reader.GetString("phone")
                };
            }

            return null;
        }

        public async Task<int> SaveUserAsync(UserModel user)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            using var command = new MySqlCommand("INSERT INTO users (name, email, password, phone) VALUES (@Name, @Email, @Password, @Phone)", connection);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Phone", user.Phone);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task AddExpenseAsync(TransactionModel model)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            var cmd = new MySqlCommand("INSERT INTO expenses (user_id, amount, date, category, comment) VALUES (@userId, @amount, @date, @cat, @com)", connection);
            cmd.Parameters.AddWithValue("@userId", model.UserId);
            cmd.Parameters.AddWithValue("@amount", model.Amount);
            cmd.Parameters.AddWithValue("@date", model.Date);
            cmd.Parameters.AddWithValue("@cat", model.Category);
            cmd.Parameters.AddWithValue("@com", model.Comment);
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public async Task<List<TransactionModel>> GetExpensesByUserAsync(int userId)
        {
            var result = new List<TransactionModel>();
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var cmd = new MySqlCommand("SELECT * FROM expenses WHERE user_id = @id", connection);
            cmd.Parameters.AddWithValue("@id", userId);
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new TransactionModel
                {
                    Id = reader.GetInt32("id"),
                    UserId = userId,
                    Amount = reader.GetDecimal("amount"),
                    Date = reader.GetDateTime("date"),
                    Category = reader.GetString("category"),
                    Comment = reader.GetString("comment")
                });
            }

            await connection.CloseAsync();
            return result;
        }

        public async Task AddIncomeAsync(TransactionModel model)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            var cmd = new MySqlCommand("INSERT INTO incomes (user_id, amount, date, category, comment) VALUES (@userId, @amount, @date, @cat, @com)", connection);
            cmd.Parameters.AddWithValue("@userId", model.UserId);
            cmd.Parameters.AddWithValue("@amount", model.Amount);
            cmd.Parameters.AddWithValue("@date", model.Date);
            cmd.Parameters.AddWithValue("@cat", model.Category);
            cmd.Parameters.AddWithValue("@com", model.Comment);
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public async Task<List<TransactionModel>> GetIncomeByUserAsync(int userId)
        {
            var result = new List<TransactionModel>();
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var cmd = new MySqlCommand("SELECT * FROM incomes WHERE user_id = @id", connection);
            cmd.Parameters.AddWithValue("@id", userId);
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new TransactionModel
                {
                    Id = reader.GetInt32("id"),
                    UserId = userId,
                    Amount = reader.GetDecimal("amount"),
                    Date = reader.GetDateTime("date"),
                    Category = reader.GetString("category"),
                    Comment = reader.GetString("comment")
                });
            }

            await connection.CloseAsync();
            return result;
        }

        public async Task AddCategoryAsync(CategoryModel cat)
        {
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            var cmd = new MySqlCommand("INSERT INTO categories (id, title, userId, isDefault, type) VALUES (@id, @ti, @us, @def, @ty)", connection);
            cmd.Parameters.AddWithValue("@id", cat.Id);
            cmd.Parameters.AddWithValue("@ti", cat.Title);
            cmd.Parameters.AddWithValue("@us", cat.UserId);
            cmd.Parameters.AddWithValue("@def", cat.IsDefault);
            cmd.Parameters.AddWithValue("@ty", cat.Type);
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }

        public async Task<List<CategoryModel>> GetCategoryByUserAsync(int userId, string type)
        {
            var result = new List<CategoryModel>();

            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var sql = "SELECT id, title, userId, `type`, isDefault FROM categories WHERE userId = @id AND `type` = @type";
            using var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.Parameters.AddWithValue("@type", type.Trim());

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new CategoryModel
                {
                    Id = reader.GetInt32("id"),
                    Title = reader.GetString("title"),
                    UserId = reader.GetInt32("userId"),
                    Type = reader.IsDBNull(reader.GetOrdinal("type")) ? null : reader.GetString("type"),
                    IsDefault = reader.GetBoolean("isDefault")
                });
            }

            return result;
        }
    }
}
