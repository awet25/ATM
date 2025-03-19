
using ATMApp.Interfaces;
using ATMApp.Models;
using System.Threading.Tasks;   
namespace ATMApp.Services{

    public class ClientService :IClientService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        public ClientService(IAccountRepository accountRepository, ITransactionRepository transactionRepository){
        _accountRepository=accountRepository;
        _transactionRepository=transactionRepository;

        }
        public async Task<bool> Deposit(int ClientId, decimal amount)
         {
            if (amount <= 0)
            {
                Console.WriteLine("Deposit amount must be greater than zero.");
                return false;
            }

            var account = await _accountRepository.GetAccountByClientID(ClientId);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return false;
            }
            
             if (account.status.Equals(AccountStatus.Disabled)){
                Console.WriteLine("Sorry this Account was Disabled pls visit our office or call us");
                return false;
            }
              account.IntialBalance+=amount;
            await _accountRepository.UpdateAccount(account);

            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = amount,
                type = TransactionType.Deposit
            };
            await _transactionRepository.AddTransaction(transaction);

            
            Console.WriteLine($" Cash Deposited Successfully ");
            Console.WriteLine($"Account #{account.Id}");
            Console.WriteLine($"Date:{DateTime.Now.ToString("MM/dd/yyyy")}");
            Console.WriteLine($"Deposited : {amount}");
            Console.WriteLine($"Balanace :{account.IntialBalance:F2}");

            return true;
        }

        public async Task  GetBalance(int ClientId)
        {
           var account = await _accountRepository.GetAccountByClientID(ClientId);
           if(account == null)
           {
            Console.WriteLine("Account not found.");
            
           } 
            else if (account.status.Equals(AccountStatus.Disabled)){
                Console.WriteLine("Sorry this Account was Disabled pls visit our office or call us");
                return;
            }
           else{
             Console.WriteLine(" Account Info");
            Console.WriteLine($"Account #{account.Id}");
            Console.WriteLine($"Date:{DateTime.Now.ToString("MM/dd/yyyy")}");
            Console.WriteLine($"Balanace :{account.IntialBalance:F2}");
           }
           
        }

        public async Task<List<Transaction>> GetTransactionHistory(int accountId)
        {
            return await _transactionRepository.GetTransactionsByAccountId(accountId);
        }

        public async  Task<bool> Withdraw(int ClientID, decimal amount)
        {
           
            var account = await _accountRepository.GetAccountByClientID(ClientID);
            if(account==null)
            {
                Console.WriteLine($"Client {ClientID} does not exist");
                return false;
            } 
             else if (account.status.Equals(AccountStatus.Disabled)){
                Console.WriteLine("Sorry this Account was Disabled pls visit our office or call us");
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
                AccountId=account.Id,
                Amount=amount,
                type=TransactionType.Withdrawal
            };
            await _transactionRepository.AddTransaction(transaction);
            Console.WriteLine($" Cash Successfully Withdrawn ");
            Console.WriteLine($"Account #{account.Id}");
            Console.WriteLine($"Date:{DateTime.Now.ToString("MM/dd/yyyy")}");
            Console.WriteLine($"Withdrawn : {amount}");
            Console.WriteLine($"Balanace :{account.IntialBalance:F2}");
        return true;
        }

        
    }
}