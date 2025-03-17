using ATMApp.Models;

namespace ATMApp.DTOs{
public class CreateAccountDto{
     
        public int ClientID { get; set; }
        public decimal IntialBalance { get; set; } = 0.0m;
        public AccountStatus status { get; set; }=AccountStatus.Active;
    }

}

