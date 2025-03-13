using FluentValidation;
using ATMApp.Models;

namespace ATMApp.Validators
{
    public class UserLoginValidator : AbstractValidator<User>
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