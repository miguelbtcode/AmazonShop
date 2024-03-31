using MediatR;

namespace Ecommerce.Application.Features.Auth.Users.Commands.SendPassword;

public class SendPasswordCommand : IRequest<string>
{
    public string? Email { get; set; }
}