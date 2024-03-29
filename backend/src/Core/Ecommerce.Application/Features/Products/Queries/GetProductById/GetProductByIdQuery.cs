using Ecommerce.Application.Features.Products.Queries.Vms;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductVm>
{
    public int ProductId { get; set; }
    public GetProductByIdQuery(int productId)
    {
        ProductId = productId == 0 ? throw new ArgumentNullException(nameof(productId)) : productId;
    }
}