using ATMApp.DTOs;
using FluentValidation;
namespace ATMApp.Validators{
    public class AddNewuserValidator:AbstractValidator<CreateUserDto>
    {
        public AddNewuserValidator()
        {
            RuleFor(user => user.Login)
                .NotEmpty().WithMessage("Login is required.");
                

            RuleFor(user => user.PinCode)
                .NotEmpty().WithMessage("PIN code is required.")
                .Length(5).WithMessage("PIN code must be exactly 5 digits.");

            RuleFor(user => user.HolderName)
                .NotEmpty().WithMessage("Holder Name is required.")
                .MaximumLength(50).WithMessage("Holder Name cannot exceed 50 characters.");

            RuleFor(user => user.Role)
                .IsInEnum().WithMessage("Invalid role. Choose either 'Admin' or 'Client'.");
        }
    }
}