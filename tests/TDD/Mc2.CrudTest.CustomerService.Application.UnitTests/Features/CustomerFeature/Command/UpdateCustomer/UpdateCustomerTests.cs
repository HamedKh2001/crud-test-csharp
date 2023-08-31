using AutoMapper;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.customerFeature.Commands.Updatecustomer;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.UpdateUser;
using Mc2.CrudTest.CustomerService.Application.UnitTests.Common;
using Mc2.CrudTest.CustomerService.Domain.Entities;
using Moq;
using SharedKernel.Contracts.Infrastructure;
using SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace Mc2.CrudTest.CustomerService.Application.UnitTests.Features.CustomerFeature.Command.UpdateCustomer
{
    public class UpdateCustomerTests
    {
        private readonly IMapper _mapper;
        public UpdateCustomerTests()
        {
            _mapper = _mapper.GetMapper();
        }

        [Fact]
        public async Task Handle_ExistingCustomer_UpdatesCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var updateCustomerCommand = new UpdateCustomerCommand { Id = customerId };
            var existingCustomer = new Customer();

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(repo => repo.GetAsync(customerId, CancellationToken.None))
                .ReturnsAsync(existingCustomer);


            var mockEventSource = new Mock<IEventSource>();

            var handler = new UpdateCustomerCommandHandler(
                mockCustomerRepository.Object,
                _mapper,
                mockEventSource.Object
            );

            // Act
            await handler.Handle(updateCustomerCommand, CancellationToken.None);

            // Assert
            mockCustomerRepository.Verify(repo => repo.UpdateAsync(existingCustomer, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingCustomer_ThrowsNotFoundException()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var updateCustomerCommand = new UpdateCustomerCommand { Id = customerId };

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(repo => repo.GetAsync(customerId, CancellationToken.None))
                .ReturnsAsync((Customer)null); // Return null to simulate non-existing customer

            var mockEventSource = new Mock<IEventSource>();

            var handler = new UpdateCustomerCommandHandler(
                mockCustomerRepository.Object,
                _mapper,
                mockEventSource.Object
            );

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(updateCustomerCommand, CancellationToken.None));
        }
    }
}
