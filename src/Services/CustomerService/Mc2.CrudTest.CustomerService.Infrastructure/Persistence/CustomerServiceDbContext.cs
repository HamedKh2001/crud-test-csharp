using Mc2.CrudTest.CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Mc2.CrudTest.CustomerService.Infrastructure.Persistence
{
    public class CustomerServiceDbContext : DbContext
    {

        public CustomerServiceDbContext()
        {
        }

        public CustomerServiceDbContext(DbContextOptions<CustomerServiceDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
