using ATMApp.Models;
using ATMApp.DTOs;
using ATMApp.Interfaces;
using ATMApp.Repositories;
using FluentValidation;

namespace ATMApp.Services
{
public class AdminServices:IAdminservices
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserDto> _userValidator;
    public AdminServices(IUserRepository userRepository, IValidator<CreateUserDto> userValidator){
        _userRepository = userRepository;
        _userValidator = userValidator;
    }

   public async Task<bool> AddUser(CreateUserDto userDto){
      var validationResult= _userValidator.Validate(userDto);
      if (!validationResult.IsValid){
        foreach(var err in validationResult.Errors){
            Console.WriteLine(err.ErrorMessage);
        }
        return false;
      }
    var existingUser=await _userRepository.GetUserBylogin(userDto.Login);
    if (existingUser!=null){
        Console.WriteLine("User with this login already exists.");
        return false;
    }
    var newUser= new User
    {
     HolderName=userDto.HolderName,
     Login = userDto.Login,
     PinCode=userDto.PinCode,
     Role=userDto.Role,
    };
    await _userRepository.AddUser(newUser);
    Console.WriteLine($"user : {newUser.HolderName} successfully created with {newUser.Role}");
    return true;
   }

}



}