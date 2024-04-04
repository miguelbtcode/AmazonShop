using FluentValidation;

namespace Ecommerce.Application.Features.Auths.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre no puede ser nulo");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido no puede ser nulo");
    }
}