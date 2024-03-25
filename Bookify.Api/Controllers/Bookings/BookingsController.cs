using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Bookings;

[ApiController]
[Route("api/bookings")]
public class BookingsController : Controller
{
    private readonly ISender _mediator;

    public BookingsController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ReserveBooking(
        ReserveBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.UserId,
            request.ApartmentId,
            request.StartDate,
            request.EndDate);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), routeValues: new {id = result.Value}, value: result.Value);
    }
}