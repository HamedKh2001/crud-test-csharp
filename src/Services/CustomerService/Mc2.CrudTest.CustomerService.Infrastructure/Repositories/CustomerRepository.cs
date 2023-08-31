using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Domain.Entities;
using Mc2.CrudTest.CustomerService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Common;
using SharedKernel.Extensions;

namespace Mc2.CrudTest.CustomerService.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerServiceDbContext _context;

        public CustomerRepository(CustomerServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Customers.AsTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return customer;
        }

        public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken)
        {
            _context.SetModified(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Customer customer, CancellationToken cancellationToken)
        {
            _context.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PaginatedResult<Customer>> GetAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = _context.Customers.AsNoTracking();
            query = query.OrderBy(q => q.FirstName);
            return await query.ToPagedListAsync(pageNumber, pageSize, cancellationToken);
        }

        public Task<Customer> GetByEmailAsync(string email)
        {
            return _context.Customers.AsNoTracking().FirstOrDefaultAsync(q => q.Email == email);
        }

        public async Task<bool> IsUniqueCustomerAsync(string firstName, string lastName, DateTime dateOfBirth)
        {
            return await _context.Customers.FirstOrDefaultAsync(q => q.FirstName == firstName && q.LastName == lastName && q.DateOfBirth == dateOfBirth) == null;
        }
    }
}
