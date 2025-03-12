using Microsoft.EntityFrameworkCore.Diagnostics;


namespace ATMApp.Models{
public class User{
    public int Id { get; set; }
    public string HolderName {get; set;}=string.Empty;
    public string Login {get; set;}=string.Empty;
    public string PIN {get; set;}=string.Empty;
    public Decimal Balance {get; set;}=0.0m;
    public string Status{get; set;}=string.Empty;
    public string Role{get; set;}=string.Empty;
    
}
}