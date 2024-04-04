using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateAdminUser;

public class UpdateAdminUserCommandValidator : AbstractValidator<UpdateAdminUserCommand>
{
    public UpdateAdminUserCommandValidator()
    {
        RuleFor(x => x.Nombre)
        .NotEmpty().WithMessage("El nombre no puede estar vacio");

        RuleFor(x => x.Apellido)
        .NotEmpty().WithMessage("El apellido no puede estar vacio");

        RuleFor(x => x.Telefono)
        .NotEmpty().WithMessage("El telefono no puede estar vacio");
    }
}