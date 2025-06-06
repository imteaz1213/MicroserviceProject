using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ordering.Application.Features.Orders.Commands.CreateOrder.UpdateOrder;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0).WithMessage("Please enter oder id");
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
