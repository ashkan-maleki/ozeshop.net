using Catalog.Query.Application.DTOs;
using Catalog.Query.Application.Queries;
using MediatR;

namespace Catalog.Query.Application.Handlers;

public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, IReadOnlyList<ProductVm>>
{
    public Task<IReadOnlyList<ProductVm>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}