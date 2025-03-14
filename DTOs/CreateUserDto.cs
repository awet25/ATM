using ATMApp.Models;

namespace ATMApp.DTOs
{
    public class CreateUserDto:UserLoginDTO
    {

        public string HolderName { get; set; }
        public UserRole Role { get; set; }=UserRole.Client;

    }
}
