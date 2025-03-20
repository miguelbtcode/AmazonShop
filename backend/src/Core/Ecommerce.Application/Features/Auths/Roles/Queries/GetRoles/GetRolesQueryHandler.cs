using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Auths.Roles.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<string>>
{
    private readonly RoleManager<IdentityRole> roleManager;

    public GetRolesQueryHandler(RoleManager<IdentityRole> roleManager)
    {
        this.roleManager = roleManager;
    }

    public async Task<List<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles.ToListAsync(cancellationToken);
        return roles.OrderBy(x => x.Name)
                    .Select(x => x.Name!)
                    .ToList<string>();
    }
}