using ATMApp.DTOs;

namespace ATMApp.Interfaces{

public interface IAuthService
{
    Task<bool> Login(UserLoginDTO userLogin);
    void Exit();
}

}
