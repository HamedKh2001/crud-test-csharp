using FluentValidation;
using SharedKernel.Extensions;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;

namespace Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.CreateUser
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(p => p).MustAsync(BeUniqueCustomerAsync).WithMessage("The specified customer already exists.");
            RuleFor(p => p).MustAsync(BeUniqueEmail).WithMessage("The specified email already exists.");

            RuleFor(p => p.FirstName)
                .NotNull().NotEmpty().WithMessage("The {firstName} is required.")
                .MaximumLength(100).WithMessage("The firstName must not exceed 100 characters.");

            RuleFor(p => p.LastName)
                .NotNull().NotEmpty().WithMessage("The {lastName} is required.")
                .MaximumLength(100).WithMessage("The lastName must not exceed 100 characters.");

            RuleFor(p => p.PhoneNumber).NotNull().Must(p => p.ValidatePhoneNumber()).WithMessage("{phoneNumber} is required.");
        }

        public async Task<bool> BeUniqueCustomerAsync(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            return await _customerRepository.IsUniqueCustomerAsync(command.FirstName, command.LastName, command.DateOfBirth);
        }

        public async Task<bool> BeUniqueEmail(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetByEmailAsync(command.Email) == null;
        }
    }
}
