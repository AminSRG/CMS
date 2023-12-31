using Microsoft.AspNetCore.Mvc;
using MediatR;
using FluentResults;
using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Application.Customer.Query;

namespace CustomerManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateCustomer")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<Result<bool>>> CreateCustomer(CreateCustomerCommand param)
        {
            var result = await _mediator.Send(param);

            if (result.IsSuccess)return Ok(result);
            else return BadRequest(result.ToResult());
        }

        [HttpPut("UpdateCustomer")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<Result<bool>>> UpdateCustomer(UpdateCustomerCommand param)
        {
            Result<bool> result = await _mediator.Send(param);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ToResult());
        }

        [HttpDelete("DeleteCustomer")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<ActionResult<Result<bool>>> DeleteCustomer(DeleteCustomerCommand param)
        {
            Result<bool> result = await _mediator.Send(param);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ToResult());
        }

        [HttpGet("GetCustomerById")]
        [ProducesResponseType(typeof(Result<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<Result<CustomerDto>>> GetCustomerById([FromBody] GetCustomerByIdQuery param)
        {
            Result<CustomerDto> result = await _mediator.Send(param);

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ToResult());
        }

        [HttpGet("GetAllCustomers")]
        [ProducesResponseType(typeof(Result<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<Result<List<CustomerDto>>>> GetAllCustomers()
        {
            Result<List<CustomerDto>> result = await _mediator.Send(new GetAllCustomersQuery());

            if (result.IsSuccess) return Ok(result);
            else return BadRequest(result.ToResult());
        }
    }
}

