using AutoMapper;
using Ecommerce.Application.Features.Images.Queries.Vms;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Reviews.Queries.Vms;
using Ecommerce.Domain;

namespace Ecommerce.Application.Mappings;

using Features.Categories.Vms;
using Features.Countries.Vms;
using Features.Products.Commands.CreateProduct;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductVm>()
            .ForMember(vm => vm.CategoryNombre, opt => opt.MapFrom(src => src.Category!.Nombre))
            .ForMember(vm => vm.NumeroReviews, opt => opt.MapFrom(src => src.Reviews == null ? 0 : src.Reviews.Count));

        CreateMap<Image, ImageVm>();
        CreateMap<Review, ReviewVm>();
        CreateMap<Country, CountryVm>();
        CreateMap<Category, CategoryVm>();
        CreateMap<CreateProductCommand, Product>();
        CreateMap<CreateProductImageCommand, Image>();
    }
}