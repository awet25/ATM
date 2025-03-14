using FluentValidation;
using ATMApp.Models;
using ATMApp.DTOs;

namespace ATMApp.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginValidator()
        {
            RuleFor(user=>user.Login)
            .NotEmpty().WithMessage("Login is required.");
   
        RuleFor(user=>user.PinCode)
        .NotEmpty().WithMessage("PIN code is required. ")
        .Length(5).WithMessage("PIN code must be exactly 5 digits.");


        }
    }
}