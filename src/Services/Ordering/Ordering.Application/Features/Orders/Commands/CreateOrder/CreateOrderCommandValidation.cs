using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidation : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidation()
        {
            RuleFor(c => c.UserName).NotEmpty().WithMessage("Please Enter User Name")
                                    .NotNull()
                                    .EmailAddress().WithMessage("Username should be valid email");
            RuleFor(c => c.FirstName).NotEmpty().WithMessage("Please enter first name")
                                     .MaximumLength(100).WithMessage("The message is greater than 100 char");
            RuleFor(c => c.TotalPrice).GreaterThan(0).WithMessage("Total message should be greater than zero");
            RuleFor(c => c.EmailAddress).EmailAddress().WithMessage("Email address should be a valid email");
        }
    }
}
