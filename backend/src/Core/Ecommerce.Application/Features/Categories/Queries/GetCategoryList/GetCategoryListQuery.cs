namespace Ecommerce.Application.Features.Categories.Queries.GetCategoryList;

using MediatR;
using Vms;

public class GetCategoryListQuery : IRequest<IReadOnlyList<CategoryVm>>
{
    
}
