using ATMApp.DTOs;

namespace ATMApp.Interfaces
{
 public interface IAdminservices{
    Task<bool> AddUser(CreateUserDto createUserDto);
 }   
}