using ATMApp.DTOs;
using ATMApp.Models;

namespace ATMApp.Interfaces
{
 public interface IAccountRepository{
        Task<Account> CreateAccount(Account account);
        Task<Account>GetAccountByClientID(int ClientId);
        Task<Account>GetAccountById(int id);
        Task<Account> UpdateAccount(Account account);
        Task<bool>DeleteAccountById(int AccountId);
    }   
}