using AutoMapper;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.CreateUser;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser;
using Mc2.CrudTest.CustomerService.Domain.Entities;
using Moq;
using SharedKernel.Contracts.Infrastructure;
using Shouldly;
using Xunit;

namespace Mc2.CrudTest.CustomerService.Application.UnitTests.Features.CustomerFeature.Command.CreateCustomer
{
    public class CreateCustomerTests
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsCustomerDto()
        {
            // Arrange
            var request = new CreateCustomerCommand
            {
                FirstName = "hamed",
                LastName = "kh",
                PhoneNumber = "09398376015",
                Email = "Hamed.30sharp@gmail.com",
                DateOfBirth = new DateTime(2001, 2, 10),
                BankAccountNumber = "123456"
            };
            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                FirstName = "hamed",
                LastName = "kh",
                PhoneNumber = "09398376015",
                Email = "Hamed.30sharp@gmail.com",
                DateOfBirth = new DateTime(2001, 2, 10),
                BankAccountNumber = "123456"
            };
            var customerDto = new CustomerDto()
            {
                Id = Guid.NewGuid(),
                FirstName = "hamed",
                LastName = "kh",
                PhoneNumber = "09398376015",
                Email = "Hamed.30sharp@gmail.com",
                DateOfBirth = new DateTime(2001, 2, 10),
                BankAccountNumber = "123456"
            };

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<Customer>(request)).Returns(customer);
            mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

            var mockCustomerRepository = new Mock<ICustomerRepository>();
            mockCustomerRepository.Setup(repo => repo.CreateAsync(It.IsAny<Customer>(), CancellationToken.None))
                .ReturnsAsync(customer);

            var mockEventSource = new Mock<IEventSource>();

            var handler = new CreateCustomerCommandHandler(
                mockCustomerRepository.Object,
                mockMapper.Object,
                mockEventSource.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.ShouldBe(customerDto);
        }
    }
}
