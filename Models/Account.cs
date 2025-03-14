
namespace ATMApp.Models{
public class Account{
    public int Id { get; set; }
    public int ClientID { get; set; }
    public decimal IntialBalance {get; set; }=0.0m;
    public AccountStatus status { get; set; }
}
public enum AccountStatus{
    Active,
    Inactive
}

}