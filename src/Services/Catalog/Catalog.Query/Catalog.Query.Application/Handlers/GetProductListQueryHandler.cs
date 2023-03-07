using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Catalog.Query.Application.DTOs;
using Catalog.Query.Application.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Query.Application.Handlers;

public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, IReadOnlyList<ProductVm>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;
    private readonly ILogger<GetProductListQueryHandler> _logger;

    public GetProductListQueryHandler(IMapper mapper, IProductRepository repository,
        ILogger<GetProductListQueryHandler> logger)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IReadOnlyList<ProductVm>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products = await _repository.GetProductsAsync();
        return _mapper.Map<List<ProductVm>>(products);
    }
}