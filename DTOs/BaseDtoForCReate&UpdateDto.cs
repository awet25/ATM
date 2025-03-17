using ATMApp.Models;

namespace ATMApp.DTOs{

public class BaseDto{

        public string Login { get; set;} 
        public string PinCode { get; set;}
        public string HolderName { get; set; }
        public UserRole Role { get; set; }=UserRole.Client; 
}

}