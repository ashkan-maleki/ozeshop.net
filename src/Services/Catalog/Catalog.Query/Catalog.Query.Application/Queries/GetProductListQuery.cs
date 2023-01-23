using Catalog.Query.Application.DTOs;
using MediatR;

namespace Catalog.Query.Application.Queries;

public record GetProductListQuery : IRequest<IReadOnlyList<ProductVm>>;