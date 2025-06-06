using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contacts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Models;

namespace Ordering.Application.Features.Orders.Commands.CreateOrder
{
    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        IOrderRepository _orderRepository;
        IMapper _mapper;
        IEmailService _emailService;
        public CreateOrderCommandHandler(IOrderRepository orderRepository,IMapper mapper, IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);

            bool isOrderPlaced = await _orderRepository.AddAsync(order);

            if (isOrderPlaced) 
            {
                Email email = new Email();
                email.To = order.UserName;
                email.Subject = "Your order has been placed";
                email.Body = $"Dear {order.FirstName + " " + order.LastName} <br/><br/> We are excited for you to received your order #{order.Id} and with notify you one it's way. <br/> Thank you for ordering form Imteaz";
                await _emailService.SendEmailAsync(email);
            }
            return isOrderPlaced;   
        }
    }
}
