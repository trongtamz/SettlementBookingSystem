using MediatR;
using SettlementBookingSystem.Application.Bookings.Dtos;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<BookingDto>
    {
        public string Name { get; set; }
        public string BookingTime { get; set; }
    }
}
