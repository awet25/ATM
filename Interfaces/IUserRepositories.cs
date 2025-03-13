using ATMApp.Models;

namespace ATMApp.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserBylogin(string login);
        Task AddUser(User user);
    }
}