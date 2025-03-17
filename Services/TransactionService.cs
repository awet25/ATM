
using ATMApp.Interfaces;
using ATMApp.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Threading.Tasks;
namespace ATMApp.Services{

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        public TransactionService(IAccountRepository accountRepository, ITransactionRepository transactionRepository){
        _accountRepository=accountRepository;
        _transactionRepository=transactionRepository;

        }
        public async Task<bool> Deposit(int accountId, decimal amount)
         {
            if (amount <= 0)
            {
                Console.WriteLine("Deposit amount must be greater than zero.");
                return false;
            }

            var account = await _accountRepository.GetAccountById(accountId);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return false;
            }
              account.IntialBalance+=amount;
            await _accountRepository.UpdateAccount(account);

            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                type = TransactionType.Deposit
            };
            await _transactionRepository.AddTransaction(transaction);

            Console.WriteLine($"Deposit successful! New balance: {account.IntialBalance}");
            return true;
        }

        public async Task<decimal> GetBalance(int accountId)
        {
           var account = await _accountRepository.GetAccountById(accountId);
           if(account == null)
           {
            Console.WriteLine("Account not found.");
            return -1;
           }
           Console.WriteLine($"Current Balance: {account.IntialBalance}");
           return account.IntialBalance;
        }

        public async Task<List<Transaction>> GetTransactionHistory(int accountId)
        {
            return await _transactionRepository.GetTransactionsByAccountId(accountId);
        }

        public async  Task<bool> Withdraw(int accountId, decimal amount)
        {
            if (amount<=0){
                Console.WriteLine("Withdrawal amount must be greater than zero");
                return false;
            }
            var account = await _accountRepository.GetAccountById(accountId);
            if(account==null)
            {
                Console.WriteLine($"Account {accountId} does not exist");
                return false;
            }
            if(account.IntialBalance < amount)
            {
                Console.WriteLine("Insufficient balance.");
                return false;
            }
            account.IntialBalance-=amount;
            await _accountRepository.UpdateAccount(account);
            var transaction= new Transaction{
                AccountId=accountId,
                Amount=amount,
                type=TransactionType.Withdrawal
            };
            await _transactionRepository.AddTransaction(transaction);
            Console.WriteLine($"Withdrawal successful! New balance: {account.IntialBalance} .");
        return true;
        }

       
    }
}