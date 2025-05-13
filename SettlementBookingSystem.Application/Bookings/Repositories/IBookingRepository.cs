using System;
using System.Collections.Generic;

public interface IBookingRepository
{
    IDictionary<string, IEnumerable<Guid>> GetAllBookings();
    int GetBookingCount(string slot);
    void AddBooking(string slot, Guid bookingId);
}
