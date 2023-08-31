using FluentAssertions;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.DeleteUser;
using Moq;
using TechTalk.SpecFlow;

namespace Mc2.CrudTest.CustomerService.Application.UnitTests.Steps.Customer
{
    [Binding]
    public class DeleteCustomerCommandSteps
    {
        private DeleteCustomerCommand _command;
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private DeleteCustomerCommandHandler _handler;

        [Given(@"a customer repository")]
        public void GivenACustomerRepository()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
        }

        [Given(@"an existing customer with the ID")]
        public void GivenAnExistingCustomerWithTheID(Table table)
        {
            var row = table.Rows[0];
            _command = new DeleteCustomerCommand
            {
                Id = Guid.Parse(row["Id"])
            };

            var existingCustomer = new Domain.Entities.Customer
            {
                Id = _command.Id,
                // Populate other properties if needed
            };

            _customerRepositoryMock.Setup(repo => repo.GetAsync(_command.Id, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(existingCustomer);
        }

        [When(@"the delete customer command is handled")]
        public async Task WhenTheDeleteCustomerCommandIsHandled()
        {
            _handler = new DeleteCustomerCommandHandler(_customerRepositoryMock.Object);
            await _handler.Handle(_command, CancellationToken.None);
        }

        [Then(@"the customer with the specified ID should be retrieved from the repository")]
        public async Task ThenTheCustomerWithTheSpecifiedIDShouldBeRetrievedFromTheRepository()
        {
            var retrievedCustomer = await _customerRepositoryMock.Object.GetAsync(_command.Id, CancellationToken.None);
            retrievedCustomer.Should().NotBeNull();
        }

        [Then(@"the customer should be deleted from the repository")]
        public void ThenTheCustomerShouldBeDeletedFromTheRepository()
        {
            _customerRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Domain.Entities.Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
