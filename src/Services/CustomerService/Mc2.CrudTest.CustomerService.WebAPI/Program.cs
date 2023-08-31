using Mc2.CrudTest.CustomerService.Application;
using Mc2.CrudTest.CustomerService.Infrastructure;
using Mc2.CrudTest.CustomerService.Infrastructure.Middlewares;
using SharedKernel.Middlewares;

namespace Mc2.CrudTest.CustomerService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddApiVersioning();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.Services.DbContextInitializer();
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}
            app.UseCustomExceptionHandler();
            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}