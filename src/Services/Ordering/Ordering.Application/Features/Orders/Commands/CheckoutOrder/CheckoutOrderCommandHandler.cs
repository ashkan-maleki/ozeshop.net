using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IAsyncOrderRepository _repo;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler
            (IAsyncOrderRepository repo, 
                IMapper mapper, 
                IEmailService emailService, 
                ILogger<CheckoutOrderCommandHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request,
            CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder = await _repo.AddAsync(orderEntity);

            _logger.LogInformation($"Order {newOrder.Id} is successfully created.");
            await SendMail(newOrder);
            return newOrder.Id.Value;
        }

        private async Task SendMail(Order order)
        {
            var email = new Email()
            {
                To = "MohammadAlimalekie@gmail.com",
                Body = $"Order was created",
                Subject = "Order was created"
            };

            try
            {
                await _emailService.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");
            }

        }
    }
}
