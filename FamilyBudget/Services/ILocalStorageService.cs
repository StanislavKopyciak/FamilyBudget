namespace FamilyBudget.Services
{
    public interface ILocalStorageService
    {
        void Set(string key, string value);
        string? Get(string key);
        void Remove(string key);
        bool Contains(string key);
    }
}