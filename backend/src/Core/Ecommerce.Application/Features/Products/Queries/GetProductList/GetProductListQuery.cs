using Ecommerce.Application.Features.Products.Queries.Vms;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.GetProductList;

public class GetProductListQuery : IRequest<IReadOnlyList<ProductVm>>
{
    
}