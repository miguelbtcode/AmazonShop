using System.Net;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Auth.Users.Commands.LoginUser;
using Ecommerce.Application.Features.Auth.Users.Commands.RegisterUser;
using Ecommerce.Application.Features.Auth.Users.Vms;
using Ecommerce.Application.Models.ImageManagement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IManageImageService _manageImageService;

    public UsuarioController(IMediator mediator, IManageImageService manageImageService)
    {
        _mediator = mediator;
        _manageImageService = manageImageService;
    }

    [AllowAnonymous]
    [HttpPost("login", Name = "Login")]
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginUserCommand request)
    {
        return await _mediator.Send(request);
    }

    [AllowAnonymous]
    [HttpPost("Register", Name = "Register")]
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> Register([FromForm] RegisterUserCommand request)
    {
        if (request.Foto is not null)
        {
            var resultImage = await _manageImageService.UploadImage(new ImageData
                                                                    {
                                                                        ImageStream = request.Foto.OpenReadStream(),
                                                                        Nombre = request.Foto.Name
                                                                    });
            
            request.FotoId = resultImage.PublicId;
            request.FotoUrl = resultImage.Url;
        }

        return await _mediator.Send(request);
    }
}