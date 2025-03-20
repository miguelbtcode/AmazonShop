namespace Ecommerce.Application.Features.Products.Commands.CreateProduct;

using MediatR;
using Microsoft.AspNetCore.Http;
using Queries.Vms;

public class CreateProductCommand : IRequest<ProductVm>
{
    public string? Nombre { get; set; }
    public decimal Precio { get; set; }
    public string? Descripcion { get; set; }
    public string? Vendedor { get; set; }
    public int Stock { get; set; }
    public string? CategoryId { get; set; }
    public IReadOnlyList<IFormFile>? Fotos { get; set; }
    public IReadOnlyList<CreateProductImageCommand>? ImagesUrl { get; set; }
}
