using Microsoft.AspNetCore.OutputCaching;

namespace ATMApp.Models
{
    public class Transaction
    {
     public int Id { get; set; }
     public int AccountId { get; set; } 
     public decimal Amount { get; set; }
     public DateTime TimeStamp {get; set; }=DateTime.UtcNow;
     public TransactionType type { get; set; }
     public Account Account { get; set; } 
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        display
    }
}