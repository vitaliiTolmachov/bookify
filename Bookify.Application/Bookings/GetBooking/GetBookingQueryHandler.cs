using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Data;
using Bookify.Domain.Shared;
using Dapper;

namespace Bookify.Application.Bookings.GetBooking;

internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetBookingQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        using (var dbConnection = _dbConnectionFactory.CreateDbConnection())
        {
            //TIP: Can be extracted to the separate view in db
            const string sql = """
                               SELECT
                                   id AS Id,
                                   apartment_id AS ApartmentId,
                                   user_id AS UserId,
                                   status AS Status,
                                   price_for_period_amount AS PriceAmount,
                                   price_for_period_currency AS PriceCurrency,
                                   cleaning_fee_amount AS CleaningFeeAmount,
                                   cleaning_fee_currency AS CleaningFeeCurrency,
                                   amenities_up_charge_amount AS AmenitiesUpChargeAmount,
                                   amenities_up_charge_currency AS AmenitiesUpChargeCurrency,
                                   total_price_amount AS TotalPriceAmount,
                                   total_price_currency AS TotalPriceCurrency,
                                   duration_start AS DurationStart,
                                   duration_end AS DurationEnd,
                                   created_on_utc AS CreatedOnUtc
                               FROM bookings
                               WHERE id = @BookingId
                               """;
            var bookingResponse = await dbConnection.QueryFirstAsync<BookingResponse>(sql, new
            {
                BookingId = request.BookingId
            });

            return bookingResponse;
        }
    }
}