using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrdersListQueryHandler
        : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
    {
        private readonly IAsyncOrderRepository _repo;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(
            IAsyncOrderRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<OrdersVm>> Handle(
            GetOrdersListQuery request,
            CancellationToken cancellationToken)
            =>
                _mapper.Map<List<OrdersVm>>(
                    await _repo.GetOrdersByUserNameAsync(
                        request.UserName)
                );

    }
}
