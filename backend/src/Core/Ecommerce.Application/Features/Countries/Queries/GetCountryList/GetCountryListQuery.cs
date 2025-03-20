namespace Ecommerce.Application.Features.Countries.Queries.GetCountryList;

using MediatR;
using Vms;

public class GetCountryListQuery : IRequest<IReadOnlyList<CountryVm>>
{
    
}
