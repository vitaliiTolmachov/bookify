using Asp.Versioning;
using Bookify.Application.Apartments.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Apartments;

[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/apartments")]
[ApiVersion(ApiVersions.V1)]
public class ApartmentController : Controller
{
    private readonly ISender _mediator;

    public ApartmentController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> SearchApartments(
        [FromQuery]SearchApartmentsRequest request,
        CancellationToken cancellationToken)
    {
        var searchQuery = new SearchApartmentsQuery(
            DateOnly.FromDateTime(request.StartDate),
            DateOnly.FromDateTime(request.EndDate));
        var result = await _mediator.Send(searchQuery, cancellationToken);
        return Ok(result);
    }
}