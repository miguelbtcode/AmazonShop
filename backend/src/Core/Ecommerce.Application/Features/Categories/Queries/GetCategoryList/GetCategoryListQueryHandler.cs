namespace Ecommerce.Application.Features.Categories.Queries.GetCategoryList;

using AutoMapper;
using Domain;
using MediatR;
using Persistence;
using Vms;

public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, IReadOnlyList<CategoryVm>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    
    public GetCategoryListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyList<CategoryVm>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        var categories = await unitOfWork.Repository<Category>().GetAsync(
            null,
            x => x.OrderBy(y => y.Nombre),
            string.Empty,
            false);

        return mapper.Map<IReadOnlyList<CategoryVm>>(categories);
    }
}