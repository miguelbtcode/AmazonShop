namespace Ecommerce.Api.Controllers;

using System.Net;
using Application.Features.Countries.Queries.GetCountryList;
using Application.Features.Countries.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class CountryController : ControllerBase
{
    private readonly IMediator mediator;
    
    public CountryController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet(Name = "GetCountries")]
    [ProducesResponseType(typeof(IReadOnlyList<CountryVm>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<CountryVm>>> GetCountries()
    {
        var query = new GetCountryListQuery();
        return Ok(await mediator.Send(query));
    }
}