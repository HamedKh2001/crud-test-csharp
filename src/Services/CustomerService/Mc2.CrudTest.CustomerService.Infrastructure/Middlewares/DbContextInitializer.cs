using Mc2.CrudTest.CustomerService.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mc2.CrudTest.CustomerService.Infrastructure.Middlewares
{
    public static class InfraMiddlewareExtension
    {
        public static void DbContextInitializer(this IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<Mc2DbContextInitializer>();
                initialiser.Initialize();
                initialiser.Seed();
            }
        }
    }
}
