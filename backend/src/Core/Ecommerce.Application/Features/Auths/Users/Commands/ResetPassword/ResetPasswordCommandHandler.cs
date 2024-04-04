using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Identity;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IAuthService _authService;

    public ResetPasswordCommandHandler(UserManager<Usuario> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var updateUser = await _userManager.FindByNameAsync(_authService.GetSessionUser()) ?? throw new BadRequestException("El usuario no existe");

        var resultValidateOldPassword = _userManager.PasswordHasher.VerifyHashedPassword(updateUser, 
                                                                                         updateUser.PasswordHash!, 
                                                                                         request.OldPassword!);

        if (resultValidateOldPassword != PasswordVerificationResult.Success)
        {
            throw new BadRequestException("El actual password ingresado es erroneo");
        }

        var hashedNewPassword = _userManager.PasswordHasher.HashPassword(updateUser, request.NewPassword!);
        updateUser.PasswordHash = hashedNewPassword;

        var resultUpdate = await _userManager.UpdateAsync(updateUser);

        if (!resultUpdate.Succeeded)
        {
            throw new Exception("No se pudo resetear el password");
        }

        return Unit.Value;
    }
}