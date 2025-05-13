using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Bookings.Dtos;
using System;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Controllers
{
    [ApiController]
    [Route("booking")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
