using System.Net;
using Ecommerce.Application.Features.Products.Queries.GetProductById;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Application.Features.Products.Queries.PaginationProducts;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Shared.Queries.Vms;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

using Application.Contracts.Infrastructure;
using Application.Features.Products.Commands.CreateProduct;
using Application.Models.Auth;
using Application.Models.ImageManagement;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IManageImageService _manageImageService;

    public ProductController(IMediator mediator, IManageImageService manageImageService)
    {
        _mediator = mediator;
        this._manageImageService = manageImageService;
    }

    [AllowAnonymous]
    [HttpGet("list", Name = "GetProductList")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductVm>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<ProductVm>>> GetProductList()
    {
        var query = new GetProductListQuery();
        var productos = await _mediator.Send(query);
        return Ok(productos);
    }

    [AllowAnonymous]
    [HttpGet("pagination", Name = "PaginationProduct")]
    [ProducesResponseType(typeof(PaginationVm<ProductVm>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<ProductVm>>> PaginationProduct
    (
        [FromQuery] PaginationProductsQuery paginationProductsQuery
    )
    {
        paginationProductsQuery.Status = ProductStatus.Activo;
        var paginationProducts = await _mediator.Send(paginationProductsQuery);
        return Ok(paginationProducts);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductVm), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductVm>> GetProductById(int id)
    {
        var query = new GetProductByIdQuery(id);
        return Ok(await _mediator.Send(query));
    }
    
    [Authorize(Roles = Role.ADMIN)]
    [HttpPost("create", Name = "CreateProduct")]
    [ProducesResponseType(typeof(ProductVm), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<ProductVm>> CreateProduct([FromForm] CreateProductCommand request)
    {
        var listFotoUrls = new List<CreateProductImageCommand>();

        if (request.Fotos is not null)
        {
            foreach (var foto in request.Fotos)
            {
                var resultImage = await _manageImageService.UploadImage(new ImageData
                {
                    ImageStream = foto.OpenReadStream(),
                    Nombre = foto.Name
                });

                var fotoCommand = new CreateProductImageCommand
                {
                    PublicCode = resultImage.PublicId, Url = resultImage.Url
                };
                
                listFotoUrls.Add(fotoCommand);
            }
            request.ImagesUrl = listFotoUrls;
        }

        return await _mediator.Send(request);
    }
}