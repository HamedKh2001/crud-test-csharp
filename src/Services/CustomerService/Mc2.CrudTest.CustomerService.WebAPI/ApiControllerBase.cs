using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mc2.CrudTest.CustomerService.WebAPI
{
    public class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null!;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
