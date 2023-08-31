using AutoMapper;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser;
using Mc2.CrudTest.CustomerService.Domain.Entities;
using MediatR;
using SharedKernel.Contracts.Infrastructure;

namespace Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.CreateUser
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IEventSource _eventSource;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository,
                                            IMapper mapper,
                                            IEventSource eventSource)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _eventSource = eventSource;
        }

        public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(request);
            customer.Id = Guid.NewGuid();

            var result = await _customerRepository.CreateAsync(customer, cancellationToken);

            _eventSource.Save("Customer", customer.Id.ToString(), new List<CreateCustomerCommand> { request });

            return _mapper.Map<CustomerDto>(result);
        }
    }
}
