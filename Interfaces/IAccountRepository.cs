using ATMApp.DTOs;
using ATMApp.Models;

namespace ATMApp.Interfaces
{
 public interface IAccountRepository{
        Task<Account> CreateAccount(Account account);
        Task<Account>GetAccountById(int id);
        Task UpdateAccount(Account account);
        Task<bool>DeleteAccountById(int AccountId);
    }   
}