using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.DeleteUser;
using Mc2.CrudTest.CustomerService.Domain.Entities;
using Moq;
using SharedKernel.Exceptions;
using Shouldly;
using Xunit;

namespace Mc2.CrudTest.CustomerService.Application.UnitTests.Features.CustomerFeature.DeleteCustomerTests
{
    public class DeleteCustomerTests
    {
        [Fact]
        public async Task Handle_ExistingCustomer_DeletesCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var deleteCustomerCommand = new DeleteCustomerCommand { Id = customerId };

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(repo => repo.GetAsync(customerId, CancellationToken.None))
                .ReturnsAsync(new Customer());

            var handler = new DeleteCustomerCommandHandler(mockCustomerRepository.Object);

            // Act
            await handler.Handle(deleteCustomerCommand, CancellationToken.None);

            // Assert
            mockCustomerRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Customer>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingCustomer_ThrowsNotFoundException()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var deleteCustomerCommand = new DeleteCustomerCommand { Id = customerId };

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(repo => repo.GetAsync(customerId, CancellationToken.None))
                .ReturnsAsync((Customer)null); // Return null to simulate non-existing customer

            var handler = new DeleteCustomerCommandHandler(mockCustomerRepository.Object);

            // Act and Assert
            await Should.ThrowAsync<NotFoundException>(async () =>
                await handler.Handle(deleteCustomerCommand, CancellationToken.None));
        }
    }
}
