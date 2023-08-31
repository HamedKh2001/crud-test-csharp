using Mc2.CrudTest.CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mc2.CrudTest.CustomerService.Infrastructure.Persistence
{
    public class Mc2DbContextInitializer
    {
        private readonly ILogger<Mc2DbContextInitializer> _logger;
        private readonly CustomerServiceDbContext _context;

        public Mc2DbContextInitializer(ILogger<Mc2DbContextInitializer> logger, CustomerServiceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                _context.Database.Migrate();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public void Seed()
        {
            try
            {
                TrySeed();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public void TrySeed()
        {
            if (_context.Customers.Count() == 0)
            {
                var customers = new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "hamed",
                    LastName = "kh",
                    PhoneNumber = "09398376015",
                    Email = "Hamed.30sharp@gmail.com",
                    DateOfBirth = DateTime.Now,
                    BankAccountNumber = "123456"
                };
                _context.Customers.Add(customers);

                _context.SaveChanges();
            }
        }
    }
}
