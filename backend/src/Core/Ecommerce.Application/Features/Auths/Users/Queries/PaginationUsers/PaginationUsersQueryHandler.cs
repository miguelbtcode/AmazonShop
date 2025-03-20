using Ecommerce.Application.Features.Shared.Queries;
using Ecommerce.Application.Features.Shared.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Application.Specifications.Users;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Auths.Users.Queries.PaginationUsers;

public class PaginationUsersQueryHandler : IRequestHandler<PaginationUsersQuery, PaginationVm<Usuario>>
{
    private readonly IUnitOfWork unitOfWork;

    public PaginationUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<PaginationVm<Usuario>> Handle(PaginationUsersQuery request, CancellationToken cancellationToken)
    {
        var userSpecificationParams = new UserSpecificationParams
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            Search = request.Search,
            Sort = request.Sort
        };

        var spec = new UserSpecification(userSpecificationParams);
        var users = await unitOfWork.Repository<Usuario>().GetAllWithSpec(spec);

        var specCount = new UserForCountingSpecification(userSpecificationParams);
        var totalUsers = await unitOfWork.Repository<Usuario>().CountAsync(specCount);

        var rounded = Math.Ceiling(Convert.ToDecimal(totalUsers) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var usersByPage = users.Count();

        var pagination = new PaginationVm<Usuario>
        {
            Count = totalUsers,
            Data = users,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            ResultByPage = usersByPage
        };

        return pagination;
    }
}