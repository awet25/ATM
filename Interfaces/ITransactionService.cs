using ATMApp.Models;

namespace ATMApp.Interfaces
{
    public interface ITransactionService{

         Task<bool> Deposit(int accountId, decimal amount);
        Task<bool> Withdraw(int accountId, decimal amount);
        Task<decimal> GetBalance(int accountId);
        Task<List<Transaction>> GetTransactionHistory(int accountId);
    } 
}