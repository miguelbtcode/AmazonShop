using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.GetProductList;

public class GetProductListQuery : IRequest<IReadOnlyList<Product>>
{
    
}