using ATMApp.DTOs;
using FluentValidation;

namespace ATMApp.Validators
 {
  public class AddNewuserValidator:AbstractValidator<CreateUserDto>
  {
    public AddNewuserValidator()
    {
        Include(new UserLoginValidator());
       RuleFor(user=>user.HolderName).NotEmpty()
       .WithMessage("Holder Name is required");

       RuleFor(user=>user.Role).
       IsInEnum().WithMessage("Invalid role. Choose either 'Admin','Client'"); 
    }
  }  
 }