using MediatR;

namespace Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser
{
    public class GetCustomerQuery : IRequest<CustomerDto>
    {
        public Guid Id { get; set; }
    }
}
