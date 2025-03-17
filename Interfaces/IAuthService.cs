using ATMApp.DTOs;
using ATMApp.Models;

namespace ATMApp.Interfaces{

public interface IAuthService
{
    Task<User> Login(UserLoginDTO userLogin);
    void Exit();
    
}

}
