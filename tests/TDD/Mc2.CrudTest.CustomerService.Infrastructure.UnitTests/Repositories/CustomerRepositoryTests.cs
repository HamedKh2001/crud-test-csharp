using Mc2.CrudTest.CustomerService.Domain.Entities;
using Mc2.CrudTest.CustomerService.Infrastructure.Persistence;
using Mc2.CrudTest.CustomerService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Mc2.CrudTest.CustomerService.UnitTests.Infrastructure.Repositories
{
    public class CustomerRepositoryTests
    {
        [Fact]
        public async Task CreateAsync_ShouldAddCustomerToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CustomerServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsync_ShouldAddCustomerToDatabase")
                .Options;

            using var context = new CustomerServiceDbContext(options);
            var repository = new CustomerRepository(context);

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                BankAccountNumber = "123456",
                PhoneNumber = "9398345656",
                FirstName = "Hamed",
                LastName = "kh",
                DateOfBirth = new DateTime(2001, 2, 10),
                Email = "hamed.30sharp@gmail.com"
            };

            // Act
            var createdCustomer = await repository.CreateAsync(customer, CancellationToken.None);

            // Assert
            createdCustomer.ShouldNotBeNull();
            context.Customers.ShouldContain(createdCustomer);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCustomerInDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CustomerServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateAsync_ShouldUpdateCustomerInDatabase")
                .Options;

            using var context = new CustomerServiceDbContext(options);
            var repository = new CustomerRepository(context);

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                BankAccountNumber = "123456",
                PhoneNumber = "9398345656",
                FirstName = "Hamed",
                LastName = "kh",
                DateOfBirth = new DateTime(2001, 2, 10),
                Email = "hamed.30sharp@gmail.com1"
            };
            await repository.CreateAsync(customer, CancellationToken.None);

            // Act
            customer.Email = "hamed.30sharp@gmail.com";
            await repository.UpdateAsync(customer, CancellationToken.None);

            // Assert
            var updatedCustomer = await context.Customers.FindAsync(customer.FirstName, customer.LastName, customer.DateOfBirth);
            updatedCustomer.Email.ShouldBe("hamed.30sharp@gmail.com");
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnCorrectCustomer()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CustomerServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "GetByEmailAsync_ShouldReturnCorrectCustomer")
                .Options;

            using var context = new CustomerServiceDbContext(options);
            var repository = new CustomerRepository(context);

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                BankAccountNumber = "123456",
                PhoneNumber = "9398345656",
                FirstName = "Hamed",
                LastName = "kh",
                DateOfBirth = new DateTime(2001, 2, 10),
                Email = "hamed.30sharp@gmail.com"
            };
            await repository.CreateAsync(customer, CancellationToken.None);

            // Act
            var foundCustomer = await repository.GetByEmailAsync("hamed.30sharp@gmail.com");

            // Assert
            foundCustomer.ShouldNotBeNull();
            foundCustomer.Id.ShouldBe(customer.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCustomerFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CustomerServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsync_ShouldRemoveCustomerFromDatabase")
                .Options;

            using var context = new CustomerServiceDbContext(options);
            var repository = new CustomerRepository(context);

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                BankAccountNumber = "123456",
                PhoneNumber = "9398345656",
                FirstName = "Hamed",
                LastName = "kh",
                DateOfBirth = new DateTime(2001, 2, 10),
                Email = "hamed.30sharp@gmail.com"
            };
            await repository.CreateAsync(customer, CancellationToken.None);

            // Act
            await repository.DeleteAsync(customer, CancellationToken.None);

            // Assert
            var deletedCustomer = await context.Customers.FindAsync(customer.FirstName, customer.LastName, customer.DateOfBirth);
            deletedCustomer.ShouldBeNull();
        }

        [Fact]
        public async Task IsUniqueCustomerAsync_ShouldReturnTrueForUniqueCustomer()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CustomerServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "IsUniqueCustomerAsync_ShouldReturnTrueForUniqueCustomer")
                .Options;

            using var context = new CustomerServiceDbContext(options);
            var repository = new CustomerRepository(context);

            // Act
            var isUnique = await repository.IsUniqueCustomerAsync("Hamed", "kh", new DateTime(1995, 6, 15));

            // Assert
            isUnique.ShouldBeTrue();
        }

        [Fact]
        public async Task IsUniqueCustomerAsync_ShouldReturnFalseForNonUniqueCustomer()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CustomerServiceDbContext>()
                .UseInMemoryDatabase(databaseName: "IsUniqueCustomerAsync_ShouldReturnFalseForNonUniqueCustomer")
                .Options;

            using var context = new CustomerServiceDbContext(options);
            var repository = new CustomerRepository(context);

            var existingCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                BankAccountNumber = "123456",
                PhoneNumber = "9398345656",
                FirstName = "Hamed",
                LastName = "kh",
                DateOfBirth = new DateTime(2001, 2, 10),
                Email = "hamed.30sharp@gmail.com"
            };
            await repository.CreateAsync(existingCustomer, CancellationToken.None);

            // Act
            var isUnique = await repository.IsUniqueCustomerAsync("Hamed", "kh", new DateTime(2001, 2, 10));
            // Assert
            isUnique.ShouldBeFalse();
        }
    }
}
