using ATMApp.Interfaces;
using ATMApp.Repositories;

namespace ATMApp.Services
{
    public class AuthService : IAuthService
    {
     private readonly IUserRepository _userRepository;
     public AuthService(IUserRepository UserRepository)
     {
        _userRepository= UserRepository;
     }
    
     public async Task<bool>Login(string Login,string PinCode) 
     {
        if (Login.Length!=5){
            Console.WriteLine("sorry password length must be 5");
            return false;
        }
        if(string.IsNullOrEmpty(Login)){
            Console.WriteLine("please Enter Login");
            return false;

        }
            
       var user =await _userRepository.GetUserBylogin(Login);
       if(user!=null && user.PinCode==PinCode){
        Console.WriteLine($"Login successFul. Welcome {user.HolderName}");
        return true;
       }
       Console.WriteLine("Invalid login credentials.");
       return false;
     }
     public void Exit(){
        Console.WriteLine("User Logged out.");
     }

        

        
    }
}