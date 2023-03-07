using AutoMapper;
using Catalog.Query.Application.DTOs;
using Catalog.Query.Domain.Entities;

namespace Catalog.Query.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductVm>().ReverseMap();
    }
}