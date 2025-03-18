using ATMApp.DTOs;
using ATMApp.Models;

namespace ATMApp.Interfaces
{
 public interface IAdminservices{
    Task<bool> AddUser(CreateUserDto createUserDto);
    Task<User>GetUserByLogin(string login);
    Task<Account>GetAccount(int id);
    Task<bool> DeleteUserAndAccount(int userId);
    Task<bool> UpdateUser(UpdateUserDto updateUserDto);

 }   
}