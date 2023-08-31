using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using MediatR;
using SharedKernel.Exceptions;

namespace Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.DeleteUser
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetAsync(request.Id, cancellationToken);
            if (customer is null)
                throw new NotFoundException(nameof(customer), request.Id);

            await _customerRepository.DeleteAsync(customer, cancellationToken);

            return Unit.Value;
        }
    }
}
