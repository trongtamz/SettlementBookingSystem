using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SettlementBookingSystem.Infrastructure.Bookings.Repositories
{
    public class InMemoryBookingRepository : IBookingRepository
    {
        private static readonly ConcurrentDictionary<string, ConcurrentBag<Guid>> Bookings = new();

        public int GetBookingCount(string slot)
        {
            return Bookings.GetOrAdd(slot, _ => new ConcurrentBag<Guid>()).Count;
        }

        public void AddBooking(string slot, Guid bookingId)
        {
            Bookings.GetOrAdd(slot, _ => new ConcurrentBag<Guid>()).Add(bookingId);
        }

        public IDictionary<string, IEnumerable<Guid>> GetAllBookings()
        {
            return Bookings.ToDictionary(
                kvp => kvp.Key,
                kvp => (IEnumerable<Guid>)kvp.Value.ToList()
            );
        }
    }
}