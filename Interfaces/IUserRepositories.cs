using ATMApp.DTOs;
using ATMApp.Models;

namespace ATMApp.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserBylogin(string login);
        Task <User>GetUserById(int id);
        Task<User> AddUser(User newUser);
        Task<bool> DeleteUserbyId(int userId);
        Task<bool> UpdateUser(User user);

    }
}