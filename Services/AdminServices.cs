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
    private readonly IValidator<CreateUserDto> _userValidator;
    public AdminServices(ATMContext aTMContext, IUserRepository userRepository,IAccountRepository accountRepository ,IValidator<CreateUserDto> userValidator){
        _context=aTMContext;
        _userRepository = userRepository;
        _userValidator = userValidator;
        _accountRepository=accountRepository;
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
   status=AccountStatus.Active,
   IntialBalance=0.0m
    
   };
     var createdAccount= await _accountRepository.CreateAccount(account);
   if( createdAccount==null){
    Console.WriteLine("sorry Account wasn't created");
    return false;
   }
     
    Console.WriteLine($"user : {newUser.HolderName} successfully created with {newUser.Role}");
    return true;
   }
 return false;
 
   }

        public async Task<bool> DeleteUserAndAccount(int userId)
        {
         using var transaction= await _context.Database.BeginTransactionAsync();
         try{
          var user= await _userRepository.GetUserById(userId);
          if (user==null)
          {
            Console.WriteLine($"User {userId} does not exist");
            return false;
          }
          var account= await _accountRepository.GetAccountById(userId);
          if(account!=null){
            var accountDeleted=await _accountRepository.DeleteAccountById(userId);
            if(!accountDeleted){
              Console.WriteLine("Failed to delete account.");
              return false;
            }
          }
          var userDeleted=await _userRepository.DeleteUserbyId(userId);
          if(!userDeleted){
            Console.WriteLine("Failed to delete User");
            return false;
          }
          await transaction.CommitAsync();
          Console.WriteLine($"Client {user.HolderName}and their account were successfully deleted.");
          return true;
         }

         catch(Exception ex){
         await transaction.RollbackAsync();
         Console.WriteLine($"Error: {ex.Message}");
         return false;
         }
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
          try{
            var user= await _userRepository.GetUserById(updateUserDto.ClientId);
            if (user==null)
            {
              Console.WriteLine("User not found");
              return false;
            }
            if (!string.IsNullOrEmpty(updateUserDto.HolderName)){
              user.HolderName = updateUserDto.HolderName;
            }
             if (!string.IsNullOrEmpty(updateUserDto.Login)){
              user.Login = updateUserDto.Login;
            }
             if (!string.IsNullOrEmpty(updateUserDto.PinCode)){
              if (updateUserDto.PinCode.Length !=5){
                  Console.WriteLine("PinCode must be exactly 5 characters long.");
                return false;
              }
              user.PinCode = updateUserDto.PinCode;
            }
           var updatedUser= await _userRepository.UpdateUser(user);
           if (!updatedUser){
            Console.WriteLine("Failed to Update Client.");
            return false;
           }
           Console.WriteLine($"Client {user.HolderName} was successfully update .");
           return true;
           
          } 
          catch(Exception ex){
            Console.WriteLine($"Error updating user:{ex.Message}");
            return false;
          } 
        }
    }



}