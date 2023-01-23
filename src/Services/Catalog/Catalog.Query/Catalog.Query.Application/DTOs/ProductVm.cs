using Catalog.Query.Domain.Entities;

namespace Catalog.Query.Application.DTOs;

public record ProductVm(
    string? Id,
    string? Name,
    string? Category,
    string? Summary,
    string? Description,
    string? ImageFile,
    Decimal? Price
);