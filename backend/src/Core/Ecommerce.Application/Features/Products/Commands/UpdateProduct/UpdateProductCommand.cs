namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct;

using CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Http;
using Queries.Vms;

public class UpdateProductCommand : IRequest<ProductVm>
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public string? Vendedor { get; set; }
    public int Stock { get; set; }
    public string? CategoryId { get; set; }

    public IReadOnlyList<IFormFile>? Fotos { get; set; }
    public IReadOnlyList<CreateProductImageCommand>? ImageUrls { get; set; }
}