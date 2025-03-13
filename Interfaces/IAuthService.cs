namespace ATMApp.Interfaces{

public interface IAuthService
{
    Task<bool> Login(string login, string pinCode);
    void Exit();
}

}
