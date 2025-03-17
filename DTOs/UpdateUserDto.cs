
using ATMApp.Models;

namespace ATMApp.DTOs{

public class UpdateUserDto
{
    public int Id { get; set; }
public string? HolderName { get; set; }
public string? Login{ get; set; }   
public string? PinCode { get; set; }
public UserRole Role { get; set; }
}    
}