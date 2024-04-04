using System.Text;
using Ecommerce.Application.Exceptions;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPasswordByToken;

public class ResetPasswordByTokenCommandHandler : IRequestHandler<ResetPasswordByTokenCommand, string>
{
    private readonly UserManager<Usuario> _userManager;

    public ResetPasswordByTokenCommandHandler(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(ResetPasswordByTokenCommand request, CancellationToken cancellationToken)
    {
        if (!string.Equals(request.Password, request.ConfirmPassword))
        {
            throw new BadRequestException("El password no es igual a la confirmacion del password");
        }

        var updateUsuario = await _userManager.FindByEmailAsync(request.Email!) ?? throw new BadRequestException("El email no esta registrado como usuario");
        
        var token = Convert.FromBase64String(request.Token!);
        var tokenResult = Encoding.UTF8.GetString(token);

        var resultResetPassword = await _userManager.ResetPasswordAsync(updateUsuario, tokenResult, request.Password!);

        if (!resultResetPassword.Succeeded)
        {
            throw new Exception("No se pudo resetear el password");
        }

        return $"Se actualizo exitosamente tu password {request.Email}";
    }
}