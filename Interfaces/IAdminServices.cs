using ATMApp.DTOs;

namespace ATMApp.Interfaces
{
 public interface IAdminservices{
    Task<bool> AddUser(CreateUserDto createUserDto);
    Task<bool> DeleteUserAndAccount(int userId);
    Task<bool> UpdateUser(UpdateUserDto updateUserDto);

 }   
}