namespace Ecommerce.Application.Features.Countries.Queries.GetCountryList;

using AutoMapper;
using Domain;
using MediatR;
using Persistence;
using Vms;

public class GetCountryListQueryHandler : IRequestHandler<GetCountryListQuery, IReadOnlyList<CountryVm>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetCountryListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }
    
    public async Task<IReadOnlyList<CountryVm>> Handle(GetCountryListQuery request, CancellationToken cancellationToken)
    {
        var countries = await unitOfWork.Repository<Country>().GetAsync(
            null, 
            x => x.OrderBy(y => y.Name), 
            string.Empty, 
            false);

        return mapper.Map<IReadOnlyList<CountryVm>>(countries);
    }
}