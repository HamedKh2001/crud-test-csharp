using MediatR;
using SharedKernel.Contracts.Infrastructure;

namespace Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.UpdateUser
{
    public class UpdateCustomerCommand : IEvent, IRequest
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
    }
}
