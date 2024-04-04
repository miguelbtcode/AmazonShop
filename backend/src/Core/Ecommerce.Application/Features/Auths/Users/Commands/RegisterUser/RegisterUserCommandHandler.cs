using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Identity;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(UserManager<Usuario> userManager, 
                                      RoleManager<IdentityRole> roleManager, 
                                      IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userExistByEmail = await _userManager.FindByEmailAsync(request.Email!) is not null;

        if (userExistByEmail)
        {
            throw new BadRequestException("El email del usuario ya existe en la base de datos");
        }

        var userExistByUsername = await _userManager.FindByNameAsync(request.Username!) is not null;

        if (userExistByUsername)
        {
            throw new BadRequestException("El username del usuario ya existe en la base de datos");
        }

        var user = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Telefono = request.Telefono,
            Email = request.Email,
            UserName = request.Username,
            AvatarUrl = request.FotoUrl
        };

        var result = await _userManager.CreateAsync(user!, request.Password!);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, AppRole.GenericUser);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponse
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Telefono = user.Telefono,
                Email = user.Email,
                Username = user.UserName,
                Avatar = user.AvatarUrl,
                Token = _authService.CreateToken(user, roles),
                Roles = roles
            };
        }

        throw new Exception($"No se pudo registrar el usuario: {result.Errors}");
    }
}