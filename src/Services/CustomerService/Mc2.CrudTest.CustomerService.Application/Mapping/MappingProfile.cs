using AutoMapper;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.CreateUser;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.UpdateUser;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser;
using Mc2.CrudTest.CustomerService.Domain.Entities;
using SharedKernel.Common;

namespace Mc2.CrudTest.CustomerService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedList<>));
            CreateMap<DateOnly, DateTime>().ConvertUsing(input => input.ToDateTime(TimeOnly.Parse("00:00 AM")));
            CreateMap<DateOnly?, DateTime?>().ConvertUsing(input => input.HasValue ? input.Value.ToDateTime(TimeOnly.Parse("00:00 AM")) : null);
            CreateMap<DateTime, DateOnly>().ConvertUsing(input => DateOnly.FromDateTime(input));
            CreateMap<DateTime?, DateOnly?>().ConvertUsing(input => input.HasValue ? DateOnly.FromDateTime(input.Value) : null);

            #region Customer
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerCommand, Customer>();
            CreateMap<UpdateCustomerCommand, Customer>();
            #endregion
        }
    }
}
