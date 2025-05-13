using System;

namespace SettlementBookingSystem.Application.Bookings.Dtos
{
    public class BookingDto
    {
        public BookingDto()
        {
            BookingId = Guid.NewGuid();
        }

        public Guid BookingId { get; }
    }
}
