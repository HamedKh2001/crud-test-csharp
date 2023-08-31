using AutoMapper;
using FluentAssertions;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.customerFeature.Commands.Updatecustomer;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.UpdateUser;
using Mc2.CrudTest.CustomerService.Application.UnitTests.Common;
using Moq;
using SharedKernel.Contracts.Infrastructure;
using TechTalk.SpecFlow;

namespace Mc2.CrudTest.CustomerService.Application.UnitTests.Steps.Customer
{
    [Binding]
    public class UpdateCustomerCommandSteps
    {
        private UpdateCustomerCommand _command;
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<IEventSource> _eventSourceMock;
        private UpdateCustomerCommandHandler _handler;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandSteps()
        {
            _mapper = _mapper.GetMapper();
        }


        [Given(@"a customer repository")]
        public void GivenACustomerRepository()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
        }

        [Given(@"an existing customer with the ID")]
        public void GivenAnExistingCustomerWithTheID(Table table)
        {
            var row = table.Rows[0];
            var existingCustomer = new Domain.Entities.Customer
            {
                Id = Guid.Parse(row["Id"]),
            };

            _customerRepositoryMock.Setup(repo => repo.GetAsync(existingCustomer.Id, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(existingCustomer);
        }

        [Given(@"a customer update command")]
        public void GivenACustomerUpdateCommand(Table table)
        {
            var row = table.Rows[0];
            _command = new UpdateCustomerCommand
            {
                Id = Guid.Parse(row["Id"]),
                Email = row["Email"],
            };
        }

        [When(@"the update customer command is handled")]
        public async Task WhenTheUpdateCustomerCommandIsHandled()
        {
            _eventSourceMock = new Mock<IEventSource>();
            _handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _mapper, _eventSourceMock.Object);
            await _handler.Handle(_command, CancellationToken.None);
        }

        [Then(@"the existing customer should be retrieved from the repository")]
        public async Task ThenTheExistingCustomerShouldBeRetrievedFromTheRepository()
        {
            var retrievedCustomer = await _customerRepositoryMock.Object.GetAsync(_command.Id, CancellationToken.None);
            retrievedCustomer.Should().NotBeNull();
        }

        [Then(@"the customer should be updated in the repository")]
        public void ThenTheCustomerShouldBeUpdatedInTheRepository()
        {
            _customerRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Then(@"the corresponding event should be saved in the event source")]
        public void ThenTheCorrespondingEventShouldBeSavedInTheEventSource()
        {
            _eventSourceMock.Verify(source => source.Save(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<IEvent>>()), Times.Once);
        }
    }
}
