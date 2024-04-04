using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Identity;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUser;

public class UpdateAdminUserCommandHandler : IRequestHandler<UpdateAdminUserCommand, Usuario>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IAuthService _authService;

    public UpdateAdminUserCommandHandler(UserManager<Usuario> userManager, 
                                         RoleManager<IdentityRole> roleManager, 
                                         IAuthService authService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
    }

    public async Task<Usuario> Handle(UpdateAdminUserCommand request, CancellationToken cancellationToken)
    {
        var updateUsuario = await _userManager.FindByIdAsync(request.Id!) ?? throw new BadRequestException("El usuario no existe");

        updateUsuario.Nombre = request.Nombre;
        updateUsuario.Apellido = request.Apellido;
        updateUsuario.Telefono = request.Telefono;

        var resultUserManager = await _userManager.UpdateAsync(updateUsuario);

        if (!resultUserManager.Succeeded)
        {
            throw new Exception("No se pudo actualizar el usuario");
        }

        var role = await _roleManager.FindByNameAsync(request.Role!) ?? throw new Exception("El rol asignado no existe");
        await _userManager.AddToRoleAsync(updateUsuario, role.Name!);

        return updateUsuario;
    }
}