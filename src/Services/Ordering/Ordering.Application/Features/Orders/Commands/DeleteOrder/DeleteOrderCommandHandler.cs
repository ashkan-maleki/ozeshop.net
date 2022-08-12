using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IAsyncOrderRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IAsyncOrderRepository repo,
            IMapper mapper,
            ILogger<CheckoutOrderCommandHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request,
            CancellationToken cancellationToken)
        {
            var order = 
                await _repo.GetByIdAsync(request.Id.Value);

            if (order == null)
            {
                _logger.LogError("Order does not exist on database");
                throw new NotFoundException(nameof(Order), request.Id);
            }

            await _repo.DeleteAsync(order);
            _logger.LogInformation($"Order {order.Id} is successfully deleted");
            return Unit.Value;
        }
    }
}
