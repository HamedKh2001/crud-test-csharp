using AutoMapper;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser;
using Mc2.CrudTest.CustomerService.Application.UnitTests.Common;
using Mc2.CrudTest.CustomerService.Domain.Entities;
using Moq;
using SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace Mc2.CrudTest.CustomerService.Tests.Application.Features.UserFeature.Queries
{
    public class GetCustomerQueryHandlerTests
    {
        private readonly IMapper _mapper;
        public GetCustomerQueryHandlerTests()
        {
            _mapper = _mapper.GetMapper();
        }

        [Fact]
        public async Task Handle_ExistingCustomer_ReturnsCustomerDto()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var getCustomerQuery = new GetCustomerQuery { Id = customerId };
            var existingCustomer = new Customer();
            var customerDto = new CustomerDto();

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(repo => repo.GetAsync(customerId, CancellationToken.None))
                .ReturnsAsync(existingCustomer);

            var handler = new GetCustomerQueryHandler(
                mockCustomerRepository.Object,
                _mapper);

            // Act
            var result = await handler.Handle(getCustomerQuery, CancellationToken.None);

            // Assert
            result.ShouldBeEquivalentTo(customerDto);
        }

        [Fact]
        public async Task Handle_NonExistingCustomer_ThrowsNotFoundException()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var getCustomerQuery = new GetCustomerQuery { Id = customerId };

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(repo => repo.GetAsync(customerId, CancellationToken.None))
                .ReturnsAsync((Customer)null); // Return null to simulate non-existing customer

            var handler = new GetCustomerQueryHandler(
                mockCustomerRepository.Object,
                _mapper);

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(getCustomerQuery, CancellationToken.None));
        }
    }
}
