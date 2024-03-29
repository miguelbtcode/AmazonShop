using AutoMapper;
using Ecommerce.Application.Features.Images.Queries.Vms;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Reviews.Queries.Vms;
using Ecommerce.Domain;

namespace Ecommerce.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductVm>()
            .ForMember(vm => vm.CategoryNombre, opt => opt.MapFrom(src => src.Category!.Nombre))
            .ForMember(vm => vm.NumeroReviews, opt => opt.MapFrom(src => src.Reviews == null ? 0 : src.Reviews.Count));

        CreateMap<Image, ImageVm>();
        CreateMap<Review, ReviewVm>();
    }
}