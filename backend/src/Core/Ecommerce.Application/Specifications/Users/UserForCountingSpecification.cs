using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Users;

public class UserForCountingSpecification : BaseSpecification<Usuario>
{
    public UserForCountingSpecification(UserSpecificationParams userParams) : base(
        x =>
        (string.IsNullOrEmpty(userParams.Search) || x.Nombre!.Contains(userParams.Search)
        || x.Apellido!.Contains(userParams.Search) || x.Email!.Contains(userParams.Search)
        )
    )
    {
        
    }
}