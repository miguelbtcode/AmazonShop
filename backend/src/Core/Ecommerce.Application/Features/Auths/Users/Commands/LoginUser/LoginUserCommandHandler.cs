using AutoMapper;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Auths.Users.Vms;
using Ecommerce.Application.Identity;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Application.Features.Auths.Users.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork; 
    private readonly IMapper _mapper;

    public LoginUserCommandHandler(UserManager<Usuario> userManager, 
                                   SignInManager<Usuario> signInManager, 
                                   RoleManager<IdentityRole> roleManager, 
                                   IAuthService authService, 
                                   IUnitOfWork unitOfWork, 
                                   IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);

        if (user is null)
        {
            throw new NotFoundException(nameof(Usuario), request.Email!);
        }

        if (!user.IsActive)
        {
            throw new Exception("El usuario esta bloqueado, contacte al admin");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password!, false);

        if (!result.Succeeded)
        {
            throw new Exception("Las credenciales del usuario son erroneas");
        }

        var direccionEnvio = await _unitOfWork.Repository<Address>().GetEntityAsync(
                                 x => x.Username == user.UserName
                             );

        var roles = await _userManager.GetRolesAsync(user);

        var authResponse = new AuthResponse
        {
            Id = user.Id,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Telefono = user.Telefono,
            Username = user.UserName,
            Email = user.Email,
            Token = _authService.CreateToken(user, roles),
            Avatar = user.AvatarUrl,
            DireccionEnvio = _mapper.Map<AddressVm>(direccionEnvio),
            Roles = roles
        };

        return authResponse;
    }
}