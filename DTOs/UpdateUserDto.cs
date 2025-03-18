
using ATMApp.Models;

namespace ATMApp.DTOs{

public class UpdateUserDto:BaseDto
{
 public int Id { get; set; }
public string? HolderName { get; set; }
public string? Login{ get; set; }   
public string? PinCode { get; set; }
public AccountStatus? status { get; set; }
}    
}