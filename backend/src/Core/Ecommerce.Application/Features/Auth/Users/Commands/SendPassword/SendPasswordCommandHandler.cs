using System.Text;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Models.Email;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Ecommerce.Application.Features.Auth.Users.Commands.SendPassword;

public class SendPasswordCommandHandler : IRequestHandler<SendPasswordCommand, string>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<Usuario> _userManager;
    private readonly IConfiguration _configuration;

    public SendPasswordCommandHandler(IEmailService emailService,
                                      UserManager<Usuario> userManager,
                                      IConfiguration configuration)
    {
        _emailService = emailService;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> Handle(SendPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);

        if(user is null)
        {
            throw new BadRequestException("El usuario no existe");
        }

        // Generate token for reset password
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var plainTextBytes = Encoding.UTF8.GetBytes(token);
        token = Convert.ToBase64String(plainTextBytes);

        // Make body email
        string urlDeploy = _configuration["Environments:UrlDevelopment"]!;
        string messageBody = $"Resetear el password, dale click aqui: {urlDeploy}/password/reset/{token}";

        var emailMessage = new EmailMessage
        {
            To = request.Email,
            Body = messageBody,
            Subject = "Cambiar el password"
        };

        var result = await _emailService.SendEmail(emailMessage, token);

        if(!result)
          throw new Exception("No se pudo enviar el email");
          
        return $"Se envio el email a la cuenta {request.Email}";
    }
}