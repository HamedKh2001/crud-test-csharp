using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser;
using MediatR;
using SharedKernel.Contracts.Infrastructure;

namespace Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.CreateUser
{
    public class CreateCustomerCommand : IEvent, IRequest<CustomerDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
    }
}
