using AutoMapper;
using BRTechGroup.SSO.Application.Features.UserFeature.Queries.GetUsers;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUsers;
using Mc2.CrudTest.CustomerService.Application.UnitTests.Common;
using Mc2.CrudTest.CustomerService.Domain.Entities;
using Moq;
using SharedKernel.Common;
using Shouldly;
using Xunit;

namespace Mc2.CrudTest.CustomerService.Tests.Application.Features.UserFeature.Queries
{
    public class GetCustomersQueryHandlerTests
    {
        private readonly IMapper _mapper;
        public GetCustomersQueryHandlerTests()
        {
            _mapper = _mapper.GetMapper();
        }

        [Fact]
        public async Task Handle_ReturnsPaginatedResultWithCustomers()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var count = 10;
            var getCustomersQuery = new GetCustomersQuery { PageNumber = pageNumber, PageSize = pageSize };
            var customerList = new PaginatedResult<Customer>(new List<Customer>(), count, pageNumber, pageSize);

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(repo => repo.GetAsync(pageNumber, pageSize, CancellationToken.None))
                .ReturnsAsync(customerList);

            var handler = new GetCustomersQueryHandler(
                mockCustomerRepository.Object,
                _mapper
            );

            // Act
            var result = await handler.Handle(getCustomersQuery, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Pagination.ShouldNotBeNull();
            result.Pagination.PageSize.ShouldBe(pageSize);
            result.Pagination.CurrentPage.ShouldBe(pageNumber);
            result.Items.Count.ShouldBe(customerList.Items.Count);
        }
    }
}
