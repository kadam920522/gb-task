using GlobalBlue.CustomerManager.Application.Create;
using GlobalBlue.CustomerManager.Application.Entities;
using GlobalBlue.CustomerManager.Application.Retrieve.GetAll;
using GlobalBlue.CustomerManager.Application.Retrieve.GetById;
using GlobalBlue.CustomerManager.Application.Update;
using GlobalBlue.CustomerManager.WebApi.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerRequestDto = GlobalBlue.CustomerManager.WebApi.DataTransferObjects.Request.CustomerDto;
using CustomerResponseDto = GlobalBlue.CustomerManager.WebApi.DataTransferObjects.Response.CustomerDto;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CustomerResponseDto>))]
        [OpenApiOperation("Fetch all customers", "This endpoint is used to fetch all available customers")]
        public async Task<IActionResult> Get()
        {
            var customers = await _sender.Send(new GetAllCustomerQuery());
            return Ok(customers.Select(customer => CustomerResponseDto.MapFrom(customer)));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
        [OpenApiOperation("Fetch the specified customer", "This endpoint tends to return the customer by the specified id.")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await _sender.Send(new GetCustomerByIdQuery(id));

            return Ok(CustomerResponseDto.MapFrom(customer));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CustomerResponseDto))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(CustomerConflicProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        [OpenApiOperation("Create a new customer", "This endpoint tends to create new customer.")]
        public async Task<IActionResult> Post([FromBody] CustomerRequestDto dto)
        {
            var command = MapToCommand(dto);
            var newCustomer = await _sender.Send(command);

            return CreatedAtAction(nameof(Get), new { id = newCustomer.Id }, CustomerResponseDto.MapFrom(newCustomer));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(CustomerConflicProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Microsoft.AspNetCore.Mvc.ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ValidationProblemDetails))]
        [OpenApiOperation("Update the specified customer", "This endpoint tends to update customer specified by it's id.")]
        public async Task<IActionResult> Put(int id, [FromBody] CustomerRequestDto dto)
        {
            var command = MapToCommand(id, dto);
            await _sender.Send(command);

            return NoContent();
        }

        private UpdateCustomerCommand MapToCommand(int id, CustomerRequestDto dto) =>
            new UpdateCustomerCommand(id, dto.FirstName, dto.Surname, dto.EmailAddress, dto.Password);

        private CreateCustomerCommand MapToCommand(CustomerRequestDto dto) =>
            new CreateCustomerCommand(dto.FirstName, dto.Surname, dto.EmailAddress, dto.Password);
    }
}
