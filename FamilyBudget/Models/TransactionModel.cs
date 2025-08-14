
namespace FamilyBudget.Models
{
    public class TransactionModel 
    {
        public int Id { get; set; }
        public string? Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; } = "";
    }
}
