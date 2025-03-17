using ATMApp.Models;

namespace ATMApp.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddTransaction(Transaction transaction);
        Task<List<Transaction>> GetTransactionsByAccountId(int accountId);
    }
}