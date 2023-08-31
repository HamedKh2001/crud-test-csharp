using Mc2.CrudTest.CustomerService.Domain.Entities;
using SharedKernel.Common;

namespace Mc2.CrudTest.CustomerService.Application.Contracts.Persistence
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken);
        Task DeleteAsync(Customer customer, CancellationToken cancellationToken);
        Task<PaginatedResult<Customer>> GetAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<Customer> GetAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer> GetByEmailAsync(string email);
        Task<bool> IsUniqueCustomerAsync(string firstName, string lastName, DateTime dateOfBirth);
    }
}
