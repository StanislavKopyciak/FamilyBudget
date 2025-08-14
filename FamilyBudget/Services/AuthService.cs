using FamilyBudget.Data;
using FamilyBudget.Models;
using Microsoft.Maui.Storage;
using System.Text.RegularExpressions;

namespace FamilyBudget.Services
{
    public class AuthService
    {
        private readonly Database _database;

        public AuthService(Database database)
        {
            _database = database;
        }

        public async Task<string> LogineAsynce(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "Email не може бути порожнім.";

            if (!IsValidEmail(email))
                return "Введено некоректний email.";

            if (email.Length > 100)
                return "Email занадто довгий.";

            if (string.IsNullOrWhiteSpace(password))
                return "Пароль не може бути порожнім.";

            if (password.Length < 8)
                return "Пароль має містити мінімум 8 символів.";

            if (password.Length > 64)
                return "Пароль занадто довгий.";


            var user = await _database.GetUserAsync(email.Trim(), password);

            if (user != null)
            {
                Preferences.Set("id", user.Id);
                Preferences.Set("name", user.Name ?? "");
                Preferences.Set("email", user.Email ?? "");
                Preferences.Set("phone", user.Phone ?? "");
                Preferences.Set("isLoggedIn", true);

                return "Ви успішно увійшли.";
            }

            return "Невірний email або пароль.";
        }

        public async Task<string> RegistrationAsync(UserModel user)
        {
            if (string.IsNullOrWhiteSpace(user.Name) || user.Name.Length < 2)
                return "Ім'я має містити щонайменше 2 символи.";

            if (user.Name.Length > 50)
                return "Ім'я занадто довге.";

            if (string.IsNullOrWhiteSpace(user.Email) || !IsValidEmail(user.Email))
                return "Неправильний формат електронної пошти.";

            if (user.Email.Length > 100)
                return "Email занадто довгий.";

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8)
                return "Пароль повинен містити щонайменше 8 символів.";

            if (user.Password.Length > 64)
                return "Пароль занадто довгий.";

            if (string.IsNullOrWhiteSpace(user.Phone) || !Regex.IsMatch(user.Phone, @"^\d{10,15}$"))
                return "Неправильний формат номера телефону.";

            var result = await _database.SaveUserAsync(user);

            return result == 1 ? "Успішно створено" : "Помилка при створенні облікового запису.";
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }

        private bool HasStrongPassword(string password)
        {
            return Regex.IsMatch(password, @"[A-Z]") &&
                   Regex.IsMatch(password, @"[a-z]") &&
                   Regex.IsMatch(password, @"[0-9]") &&
                   Regex.IsMatch(password, @"[\W_]");
        }
    }
}
