
using ATMApp.Data;
using ATMApp.DTOs;
using ATMApp.Interfaces;

using ATMApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ATMApp.Repositories
{
 public class AccountRepository:IAccountRepository
 {
    private readonly ATMContext _context;
    public AccountRepository(ATMContext context){
        _context = context;
    }

       
    public async Task<Account> CreateAccount(Account newAccount){
    var account=await _context.Account.AddAsync(newAccount);
    await _context.SaveChangesAsync();
    return account.Entity;
 }
public async Task<Account> GetAccountById(int id)
        {
            return await _context.Account.FindAsync(id);
        }
        public async Task<bool> DeleteAccountById(int AccountId)
        {
            var account=await GetAccountById(AccountId);
            if(account==null) return false;
             _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return true;


        
    }

        public async Task UpdateAccount(Account account)
        {
            _context.Account.Update(account);
            await _context.SaveChangesAsync();
        }
    }
}