using AutoMapper;
using Mc2.CrudTest.CustomerService.Application.Mapping;

namespace Mc2.CrudTest.CustomerService.Application.UnitTests.Common
{
    public static class HelperExtensions
    {
        public static IMapper GetMapper(this IMapper mapper)
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return configurationProvider.CreateMapper();
        }
    }
}
