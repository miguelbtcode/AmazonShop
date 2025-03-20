namespace Ecommerce.Api.Controllers;

using System.Net;
using Application.Features.Categories.Queries.GetCategoryList;
using Application.Features.Categories.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IMediator mediator;
    
    public CategoryController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("list", Name = "GetCategoryList")]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryVm>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<CategoryVm>>> GetCategoryList()
    {
        var query = new GetCategoryListQuery();
        return Ok(await mediator.Send(query));
    }
}
