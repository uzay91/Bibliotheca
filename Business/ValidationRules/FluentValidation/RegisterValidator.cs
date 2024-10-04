using Core.Concrete.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class RegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.TcNo).Length(11, 11);
            RuleFor(x => x.TcNo).NotEmpty().NotNull();
            RuleFor(x => x.FullName).Length(2, 50);
            RuleFor(x => x.FullName).NotEmpty().NotNull();
            RuleFor(x => x.LastName).Length(2, 50);
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress().MaximumLength(255);
            RuleFor(x => x.UserName).NotNull().NotEmpty().Length(4, 27);
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+(?:[0-9]●?){6,14}[0-9]$")
                .WithMessage("invalid phone number format")
                .MinimumLength(7)
                .WithMessage("phone number must be at least 7 digits long")
                .MaximumLength(15)
                .WithMessage("phone number must be no more than 15 digits long ")
                .NotEmpty().NotNull();

            RuleFor(p => p.Password)
                .MinimumLength(8).WithMessage("Your new password must contain at least 8 characters")
                .Matches("[A-Z]").WithMessage("Your new password must contain one or more capital letters.")
                .Matches("[a-z]").WithMessage("Your new password must contain one or more lowercase letters.")
                .Matches(@"\d").WithMessage("Your new password must contain one or more digits.")
                .Matches(@"[][""!@$%*(){}:;<>,.?/+_=|'~\\-]").WithMessage("Your password must contain one or more special characters.")
                .Matches("^[^£#^& “”]*$").WithMessage("Your password must not contain the following characters £ # ^ & “” or spaces.");
        }
    }
}
