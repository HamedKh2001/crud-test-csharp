using AutoMapper;
using BRTechGroup.SSO.Application.Features.UserFeature.Queries.GetUsers;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser;
using MediatR;
using SharedKernel.Common;

namespace Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUsers
{
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, PaginatedList<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAsync(request.PageNumber, request.PageSize, cancellationToken);
            return _mapper.Map<PaginatedList<CustomerDto>>(customers);
        }
    }
}
