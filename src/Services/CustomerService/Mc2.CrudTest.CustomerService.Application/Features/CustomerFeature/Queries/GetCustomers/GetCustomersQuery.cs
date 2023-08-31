using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser;
using MediatR;
using SharedKernel.Common;

namespace BRTechGroup.SSO.Application.Features.UserFeature.Queries.GetUsers
{
    public class GetCustomersQuery : PaginationQuery, IRequest<PaginatedList<CustomerDto>>
    {

    }
}
