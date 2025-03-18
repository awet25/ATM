using ATMApp.Models;

namespace ATMApp.DTOs
{
    public class CreateUserDto:BaseDto
    {  
       public decimal IntialBalance { get; set; } = 0.0m;
        public AccountStatus status { get; set; }=AccountStatus.Active;
    }
}
