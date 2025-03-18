using ATMApp.Models;
using ATMApp.DTOs;
using ATMApp.Interfaces;
using ATMApp.Repositories;
using FluentValidation;

using ATMApp.Data;
using Microsoft.EntityFrameworkCore;


namespace ATMApp.Services
{
public class AdminServices:IAdminservices
{
    private readonly IUserRepository _userRepository;
    private readonly ATMContext _context;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IValidator<CreateUserDto> _userValidator;
    public AdminServices(ATMContext aTMContext, IUserRepository userRepository,
    ITransactionRepository transactionRepository ,IAccountRepository accountRepository ,IValidator<CreateUserDto> userValidator){
        _context=aTMContext;
        _userRepository = userRepository;
        _userValidator = userValidator;
        _accountRepository=accountRepository;
        _transactionRepository=transactionRepository;
    }
    
   public async Task<bool> AddUser(CreateUserDto userDto){
      var validationResult= _userValidator.Validate(userDto);
      if (!validationResult.IsValid){
        foreach(var err in validationResult.Errors){
            Console.WriteLine(err.ErrorMessage);
        }
        return false;
      }
    var existingUser=await _userRepository.GetUserBylogin(userDto.Login);
    if (existingUser!=null){
        Console.WriteLine("User with this login already exists.");
        return false;
    }
    var newUser= new User
    {
     HolderName=userDto.HolderName,
     Login = userDto.Login,
     PinCode=userDto.PinCode,
     Role=userDto.Role,
    };
    var createdUser = await _userRepository.AddUser(newUser);
   if(createdUser==null)
   {
    Console.WriteLine("Failed to create User");
   }
   if (createdUser.Role== UserRole.Client){
      var account=new Account{
    ClientID=createdUser.Id,
   status=userDto.status,
   IntialBalance=userDto.IntialBalance
    
   };
     var createdAccount= await _accountRepository.CreateAccount(account);
   if( createdAccount==null){
    Console.WriteLine("sorry Account wasn't created");
    return false;
   }
     
    Console.WriteLine($"Account successfully created- the account number assigned is :{createdAccount.Id}");
    return true;
   }
 return false;
 
   }

       public async Task<bool> DeleteUserAndAccount(int accountId)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        
        var account = await _accountRepository.GetAccountById(accountId);
        if (account == null)
        {
            Console.WriteLine("Account not found. Please check the account number and try again.");
            await transaction.RollbackAsync();
            return false;
        }

        int clientId = account.ClientID;

        
        var user = await _userRepository.GetUserById(clientId);
        if (user == null)
        {
            Console.WriteLine("Associated user not found. Aborting deletion.");
            await transaction.RollbackAsync();
            return false;
        }

      
        Console.WriteLine($"You are about to delete the account held by {user.HolderName}.");
        Console.Write($"If this information is correct, please re-enter the account number ({account.Id}): ");
        
        string input = Console.ReadLine();
        int inputAgain;
        
        while (!int.TryParse(input, out inputAgain) || inputAgain <= 0)
        {
            Console.WriteLine("Invalid input! Please enter a valid account number.");
            input = Console.ReadLine();
        }

        if (inputAgain != accountId)
        {
            Console.WriteLine("Account number confirmation failed. Deletion aborted.");
            await transaction.RollbackAsync();
            return false;
        }

       
        var transactions = await _transactionRepository.GetTransactionsByAccountId(accountId);
        if (transactions.Any())
        {
            _context.Transactions.RemoveRange(transactions);
            await _context.SaveChangesAsync(); 
                    }

       
        bool accountDeleted = await _accountRepository.DeleteAccountById(accountId);
        if (!accountDeleted)
        {
            Console.WriteLine("Failed to delete account. Rolling back transaction.");
            await transaction.RollbackAsync();
            return false;
        }

    
        bool userDeleted = await _userRepository.DeleteUserbyId(clientId);
        if (!userDeleted)
        {
            Console.WriteLine("Failed to delete user. Rolling back transaction.");
            await transaction.RollbackAsync();
            return false;
        }

        await transaction.CommitAsync();
        Console.WriteLine($"User {user.HolderName}, account {accountId}, and related transactions were successfully deleted.");
        return true;
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
    Console.WriteLine($"Error: {ex.Message}");
        return false;
    }
}


        public async Task<Account> GetAccount(int id)
        {
           var existingAccount=await _context.Account.Include(a=>a.User)
           .FirstOrDefaultAsync(a=>a.Id==id);
           if(existingAccount==null){
            Console.WriteLine("Account don't exist");
            return null;
           } 
           return existingAccount;
        }

       

        public async Task<User> GetUserByLogin(string login)
        {
         var existingUser=  await _context.User.Include(u=>u.Account)
         .FirstOrDefaultAsync(u=>u.Login==login);
    if (existingUser==null){
        Console.WriteLine("User with this login already exists.");
        return null;
    }
    return existingUser;
        }

        public async Task<bool> UpdateUser(UpdateUserDto updateUserDto)
        {
          using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
        
        var account = await _accountRepository.GetAccountById(updateUserDto.Id);
        if (account == null)
        {
            Console.WriteLine("Account not found.");
            await transaction.RollbackAsync();
            return false;
        }

        
        var user = await _userRepository.GetUserById(account.ClientID);
        if (user == null)
        {
            Console.WriteLine("User not found.");
            await transaction.RollbackAsync();
            return false;
        }

        
        if (!string.IsNullOrEmpty(updateUserDto.HolderName))
        {
            user.HolderName = updateUserDto.HolderName;
        }
        if (!string.IsNullOrEmpty(updateUserDto.Login))
        {
            user.Login = updateUserDto.Login;
        }
        if (!string.IsNullOrEmpty(updateUserDto.PinCode))
        {
            if (updateUserDto.PinCode.Length != 5)
            {
                Console.WriteLine("PinCode must be exactly 5 characters long.");
                await transaction.RollbackAsync();
                return false;
            }
            user.PinCode = updateUserDto.PinCode;
        }

        
        var updatedUser = await _userRepository.UpdateUser(user);
        if (!updatedUser)
        {
            Console.WriteLine("Failed to update user.");
            await transaction.RollbackAsync();
            return false;
        }

        
        if (updateUserDto.status != null)
        {
          account.status=updateUserDto.status.Value;
            
            var updatedAccount = await _accountRepository.UpdateAccount(account);
            if (updatedAccount==null)
            {
                Console.WriteLine("Failed to update account status.");
                await transaction.RollbackAsync();
                return false;
            }
        }

        await transaction.CommitAsync();
        Console.WriteLine($"User {user.HolderName} and account {account.Id} were successfully updated.");
        return true;
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        Console.WriteLine($"Error updating user and account: {ex.Message}");
        return false;
    }
        }
    }



}