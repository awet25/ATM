using ATMApp.DTOs;
using ATMApp.Interfaces;
using ATMApp.Models;
using ATMApp.Repositories;
using ATMApp.Validators;
using FluentValidation;

namespace ATMApp.Services
{
    public class AuthService : IAuthService
    {
     private readonly IUserRepository _userRepository;
     private readonly IValidator<UserLoginDTO> _validator;
     public AuthService(IUserRepository UserRepository, IValidator<UserLoginDTO> validator)
     {
        _userRepository= UserRepository;
        _validator= validator;
     }
    
     public async Task<User>Login(UserLoginDTO userLogin) 
     {

        var validationResult=_validator.Validate(userLogin);
        if (!validationResult.IsValid)
        {
            foreach(var error in validationResult.Errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return null;
        }

       
            
       var user =await _userRepository.GetUserBylogin(userLogin.Login);
       if(user!=null && user.PinCode==userLogin.PinCode){
        
        return user;
       }
      
       return null;
     }
     public void Exit(){
        Console.WriteLine("User Logged out.");
        return;
     }

        
    }
}