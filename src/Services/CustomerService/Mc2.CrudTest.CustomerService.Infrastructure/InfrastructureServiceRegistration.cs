using EventStore.ClientAPI;
using Mc2.CrudTest.CustomerService.Application.Contracts.Persistence;
using Mc2.CrudTest.CustomerService.Infrastructure.Persistence;
using Mc2.CrudTest.CustomerService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SharedKernel.Contracts.Infrastructure;
using System.Net;

namespace Mc2.CrudTest.CustomerService.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext
            services.AddDbContext<CustomerServiceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(CustomerServiceDbContext).Assembly.FullName))
                , ServiceLifetime.Scoped);
            #endregion

            #region Event StoreDB
            var esConnection = EventStoreConnection.Create(configuration.GetConnectionString("EventStore"),
                                                           ConnectionSettings.Create().KeepReconnecting());
            esConnection.ConnectAsync().Wait();
            var store = new EventSource(esConnection);
            services.AddSingleton(esConnection);
            services.AddSingleton<IEventSource>(store);
            #endregion

            services.AddScoped<Mc2DbContextInitializer>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            ConfigureSwaggerGen(services);
            return services;
        }

        private static void ConfigureSwaggerGen(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SSO-Swagger", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme,Id = "Bearer"}},
                        new string[] {}
                    }
                });
            });
        }

        public static void ChangeModelStateInvalidModel(this ApiBehaviorOptions opt)
        {
            opt.InvalidModelStateResponseFactory =
            context => new BadRequestObjectResult(
                    JsonConvert.SerializeObject(new
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        error = string.Join(Environment.NewLine,
                            context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()
                            )
                    }));
        }
    }
}
