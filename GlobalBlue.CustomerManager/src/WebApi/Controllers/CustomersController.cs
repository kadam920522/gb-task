using GlobalBlue.CustomerManager.Application.Create;
using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Application.Retrieve.GetAll;
using GlobalBlue.CustomerManager.Application.Retrieve.GetById;
using GlobalBlue.CustomerManager.Application.Update;
using GlobalBlue.CustomerManager.WebApi.DataTransferObjects;
using GlobalBlue.CustomerManager.WebApi.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ISender _sender;

        public CustomersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Customer>))]
        public async Task<IActionResult> Get()
        {
            var customers = await _sender.Send(new GetAllCustomerQuery());
            return Ok(customers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await _sender.Send(new GetCustomerByIdQuery(id));

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(CustomerConflicProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Post([FromBody] CustomerDto dto)
        {
            var command = MapToCommand(dto);
            var newCustomer = await _sender.Send(command);

            return CreatedAtAction(nameof(Get), new { id = newCustomer.Id }, newCustomer);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(CustomerConflicProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Put(int id, [FromBody] CustomerDto dto)
        {
            var command = MapToCommand(id, dto);
            await _sender.Send(command);

            return NoContent();
        }

        private UpdateCustomerCommand MapToCommand(int id, CustomerDto dto) =>
            new UpdateCustomerCommand(id, dto.FirstName, dto.Surname, dto.EmailAddress, dto.Password);

        private CreateCustomerCommand MapToCommand(CustomerDto dto) =>
            new CreateCustomerCommand(dto.FirstName, dto.Surname, dto.EmailAddress, dto.Password);
    }
}
