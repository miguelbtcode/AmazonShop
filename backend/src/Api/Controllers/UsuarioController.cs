using System.Net;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Auth.Users.Commands.LoginUser;
using Ecommerce.Application.Features.Auth.Users.Commands.RegisterUser;
using Ecommerce.Application.Features.Auth.Users.Commands.ResetPassword;
using Ecommerce.Application.Features.Auth.Users.Commands.ResetPasswordByToken;
using Ecommerce.Application.Features.Auth.Users.Commands.SendPassword;
using Ecommerce.Application.Features.Auth.Users.Commands.UpdateUser;
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
    [HttpPost("Login", Name = "Login")]
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

    [AllowAnonymous]
    [HttpPost("ForgotPassword", Name = "ForgotPassword")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<string>> ForgotPassword([FromBody] SendPasswordCommand request)
    {
        return await _mediator.Send(request);
    }

    [AllowAnonymous]
    [HttpPost("ResetPassword", Name = "ResetPassword")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<string>> ResetPassword([FromBody] ResetPasswordByTokenCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPost("UpdatePassword", Name = "UpdatePassword")]
    [ProducesResponseType(typeof(Unit), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Unit>> UpdatePassword([FromBody] ResetPasswordCommand request)
    {
        return await _mediator.Send(request);
    }

    [HttpPut("Update", Name = "Update")]
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<AuthResponse>> Update([FromForm] UpdateUserCommand request)
    {
        if (request.Foto is not null)
        {
            var resultUploadImage = await _manageImageService.UploadImage(new ImageData
            {
                ImageStream = request.Foto.OpenReadStream(),
                Nombre = request.Foto.Name
            });

            request.FotoId = resultUploadImage.PublicId;
            request.FotoUrl = resultUploadImage.Url;
        }
        
        return await _mediator.Send(request);
    }
}