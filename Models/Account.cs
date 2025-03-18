
namespace ATMApp.Models{
public class Account{
        public int Id { get; set; }
       
        public decimal IntialBalance { get; set; } = 0.0m;
        public AccountStatus status { get; set; }=AccountStatus.Active;
         public int ClientID { get; set; }  
        public User User { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
public enum AccountStatus{
    Active,
    Disabled
}

}