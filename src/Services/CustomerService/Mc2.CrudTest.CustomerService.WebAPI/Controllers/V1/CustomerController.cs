using BRTechGroup.SSO.Application.Features.UserFeature.Queries.GetUsers;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.CreateUser;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.DeleteUser;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Commands.UpdateUser;
using Mc2.CrudTest.CustomerService.Application.Features.UserFeature.Queries.GetUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;

namespace Mc2.CrudTest.CustomerService.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<CustomerDto>>> Get([FromQuery] GetCustomersQuery query, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(query, cancellationToken));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<CustomerDto>> Get([FromRoute] GetCustomerQuery query, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(query, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var CustomerDto = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Get), new { CustomerId = CustomerDto.Id }, CustomerDto);
        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            if (Id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete([FromRoute] DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
