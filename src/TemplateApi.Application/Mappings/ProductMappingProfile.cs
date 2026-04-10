using AutoMapper;
using TemplateApi.Application.Products.DTOs;
using TemplateApi.Domain.Entities;

namespace TemplateApi.Application.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>();
    }
}