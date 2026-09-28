using FluentValidation;
using TenantApi.Dto;
using TenantApi.Models;

namespace TenantApi.Validators
{
    public class SignUpValidator : AbstractValidator<CreateUserRequest>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty()
            .WithMessage("First Name is required")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName).NotEmpty()
            .WithMessage("Last Name is required")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"\d").WithMessage("Password must contain at least one number")
            .Matches(@"[@$!%*?&]").WithMessage("Password must contain at least one special character");

            RuleFor(x => x.Email).NotEmpty().WithMessage("email is required").Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Invalid email format");
        }
    }

    public class LoginValidator : AbstractValidator<CustomLoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("email is required").Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Invalid email format");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"\d").WithMessage("Password must contain at least one number")
            .Matches(@"[@$!%*?&]").WithMessage("Password must contain at least one special character");
        }
    }

    public class LogOutValidator : AbstractValidator<LogOutRequest>
    {
        public LogOutValidator()
        {
            RuleFor(x => x.email).NotEmpty().WithMessage("email is required").Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Invalid email format");

        }
    }

    public class AdressValidator : AbstractValidator<AdressDto>
    {
        public AdressValidator()
        {
            RuleFor(x => x.StreetName).NotEmpty()
            .WithMessage("Street name is required")
            .MaximumLength(100)
            .WithMessage("Max street name is 100 characters");

            RuleFor(x => x.BuildingNumber).NotEmpty().Matches(@"^[1-9]\d*$")
            .WithMessage("House Number is required");
        }
    }
}