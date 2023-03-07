using Catalog.Query.Application.DTOs;
using MediatR;

namespace Catalog.Query.Application.Queries;

public record GetProductQuery(string Id) : IRequest<ProductVm>;