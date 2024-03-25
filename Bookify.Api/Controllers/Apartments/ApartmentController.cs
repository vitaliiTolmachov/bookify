using Bookify.Application.Apartments.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Apartments;

[ApiController]
[Route("api/apartments")]
public class ApartmentController : Controller
{
    private readonly ISender _mediator;

    public ApartmentController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> SearchApartments(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var searchQuery = new SearchApartmentsQuery(startDate, endDate);
        var result = await _mediator.Send(searchQuery, cancellationToken);
        return Ok(result);
    }
}