

using ATMApp.Data;
using ATMApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using ATMApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ATMApp.Repositories
{
    public class TransactionRepository :ITransactionRepository
    {
        private readonly ATMContext _context;
        public TransactionRepository(ATMContext context){
            _context=context;
        }

        public async Task AddTransaction(Models.Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }


        public  async Task<List<Transaction>> GetTransactionsByAccountId(int accountId)
        {
          return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .ToListAsync();  
        }
    }
}