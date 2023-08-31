using AutoMapper;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.UpdateUser;
using Mc2.CrudTest.CustomerService.Domain.Entities;
using MediatR;
using SharedKernel.Contracts.Infrastructure;
using SharedKernel.Exceptions;

namespace Mc2.CrudTest.CustomerService.Application.Features.customerFeature.Commands.Updatecustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IEventSource _eventSource;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository,
                                            IMapper mapper,
                                            IEventSource eventSource)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _eventSource = eventSource;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerToUpdate = await _customerRepository.GetAsync(request.Id, cancellationToken);
            if (customerToUpdate == null)
                throw new NotFoundException(nameof(Customer), request.Id);

            customerToUpdate = _mapper.Map(request, customerToUpdate);

            await _customerRepository.UpdateAsync(customerToUpdate, cancellationToken);

            _eventSource.Save(customerToUpdate.GetType().FullName, customerToUpdate.Id.ToString(), new List<UpdateCustomerCommand> { request });

            return Unit.Value;
        }
    }
}
