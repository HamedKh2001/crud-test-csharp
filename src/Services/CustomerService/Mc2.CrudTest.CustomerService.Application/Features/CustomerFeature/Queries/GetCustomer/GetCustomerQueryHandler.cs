using AutoMapper;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Domain.Entities;
using MediatR;
using SharedKernel.Exceptions;

namespace Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetAsync(request.Id, cancellationToken);
            if (customer == null)
                throw new NotFoundException(nameof(Customer), request.Id);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
