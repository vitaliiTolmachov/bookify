namespace Bookify.Domain.Bookings;

public record Duration
{
    public DateOnly Start { get; private set; }
    public DateOnly End { get; private set; }

    public int Days => End.DayNumber - Start.DayNumber;

    public Duration(DateOnly start, DateOnly end)
    {
        if (start > end)
            throw new ApplicationException("End date should be greater than StartDate");
        
        Start = start;
        End = end;
    }
}