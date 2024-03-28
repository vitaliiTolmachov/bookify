using System.ComponentModel.DataAnnotations;

namespace Bookify.Api.Controllers.Apartments;

public class SearchApartmentsRequest
{
    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
}