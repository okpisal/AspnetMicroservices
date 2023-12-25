using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;


namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder =await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {newOrder.ID} is Successfully Created .");
            await SendMail(newOrder);

            return newOrder.ID;
        }

        private async Task SendMail(Order order)
        {
            var email = new Email() { To = "abc@gmail.com", Body = "Order Was Creatd.", Subject = $"Order Id : {order.ID} Succesfully Created" };
            try
            {
                await _emailService.SendEmailAsync(email);
            }
            catch (Exception.ValidataionException ex)
            {

                _logger.LogError($"Order {order.ID} failed due to an error with the email service {ex.Message}");
            }
        }
    }
}
