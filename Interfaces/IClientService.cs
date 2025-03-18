using ATMApp.Models;
using System.Threading.Tasks;
namespace ATMApp.Interfaces
{
    public interface IClientService{

         Task<bool> Deposit(int accountId, decimal amount);
        Task<bool> Withdraw(int ClientID, decimal amount);
         Task GetBalance(int accountId);
        Task<List<Transaction>> GetTransactionHistory(int accountId);
    } 
}