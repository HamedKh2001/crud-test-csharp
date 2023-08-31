using AutoMapper;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.CreateUser;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser;
using Mc2.CrudTest.CustomerService.Application.UnitTests.Common;
using Moq;
using SharedKernel.Contracts.Infrastructure;
using TechTalk.SpecFlow;
using Xunit;

namespace Mc2.CrudTest.CustomerService.Application.UnitTests.Steps.Customer
{
    [Binding]
    public class CreateCustomerCommandSteps
    {
        private CreateCustomerCommand _command;
        private CustomerDto _result;
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<IEventSource> _eventSourceMock;
        private CreateCustomerCommandHandler _handler;
        private readonly IMapper _mapper;

        public CreateCustomerCommandSteps()
        {
            _mapper = _mapper.GetMapper();
        }

        [Given(@"a customer repository")]
        public void GivenACustomerRepository()
        {

            _customerRepositoryMock = new Mock<ICustomerRepository>();
        }

        [Given(@"a customer creation command")]
        public void GivenACustomerCreationCommand(Table table)
        {
            var row = table.Rows[0];
            _command = new CreateCustomerCommand
            {
                FirstName = row["FirstName"],
                LastName = row["LastName"],
                Email = row["Email"],
                DateOfBirth = DateTime.Parse(row["DateOfBirth"])
            };
        }

        [When(@"the create customer command is handled")]
        public async Task WhenTheCreateCustomerCommandIsHandled()
        {
            _eventSourceMock = new Mock<IEventSource>();
            _handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, _mapper, _eventSourceMock.Object);
            _result = await _handler.Handle(_command, CancellationToken.None);
        }

        [Then(@"a new customer should be created with the provided details")]
        public void ThenANewCustomerShouldBeCreatedWithTheProvidedDetails()
        {
            // Assert the properties of _result match the details from _command.
            Assert.Equal(_command.FirstName, _result.FirstName);
            Assert.Equal(_command.LastName, _result.LastName);
            // ...
        }

        [Then(@"the customer should be saved in the repository")]
        public void ThenTheCustomerShouldBeSavedInTheRepository()
        {
            _customerRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Domain.Entities.Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Then(@"the corresponding event should be saved in the event source")]
        public void ThenTheCorrespondingEventShouldBeSavedInTheEventSource()
        {
            _eventSourceMock.Verify(source => source.Save<IEvent>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<IEvent>>()), Times.Once);
        }
    }
}
