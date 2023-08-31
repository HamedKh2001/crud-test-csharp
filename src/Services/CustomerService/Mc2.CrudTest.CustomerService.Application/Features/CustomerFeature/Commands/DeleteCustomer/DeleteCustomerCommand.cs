using MediatR;
using SharedKernel.Contracts.Infrastructure;

namespace Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.DeleteUser
{
    public class DeleteCustomerCommand : IEvent, IRequest
    {
        public Guid Id { get; set; }
    }
}
