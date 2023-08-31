using FluentValidation;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.UpdateUser;

namespace BRTechGroup.SSO.Application.Features.UserFeature.Commands.UpdateUser
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerCommandValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(p => p.Id).NotNull().WithMessage("The {id} is required.");

            RuleFor(p => p).MustAsync(BeUniqueEmail).WithMessage("The specified email already exists.");

            RuleFor(p => p.PhoneNumber).NotNull().WithMessage("{phoneNumber} is required.");
        }

        public async Task<bool> BeUniqueEmail(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByEmailAsync(command.Email);
            var existedEmail = customer.Email;
            return existedEmail == null || existedEmail == command.Email;
        }
    }
}
