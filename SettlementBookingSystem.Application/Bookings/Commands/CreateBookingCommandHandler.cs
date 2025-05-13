using FluentValidation;
using MediatR;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Application.Exceptions;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
    {
        private readonly IBookingRepository _bookingRepository;

        private const int MaxSimultaneousBookings = 4;
        private static readonly TimeSpan Start = new(9, 0, 0);
        private static readonly TimeSpan End = new(17, 0, 0);
        private static readonly TimeSpan Duration = new(1, 0, 0);

        public CreateBookingCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var bookingMemory = _bookingRepository.GetAllBookings();
            if (!TimeSpan.TryParseExact(request.BookingTime, "hh\\:mm", CultureInfo.InvariantCulture, out var bookingTime))
                throw new ValidationException("BookingTime must be in HH:mm 24-hour format.");

            if (bookingTime < Start || bookingTime > End - Duration)
                throw new ValidationException("BookingTime is out of business hours (09:00-16:00).");

            var slot = request.BookingTime;
            if (_bookingRepository.GetBookingCount(slot) >= MaxSimultaneousBookings)
                throw new ConflictException("All settlements at this booking time are reserved.");

            var booking = new BookingDto();
            _bookingRepository.AddBooking(slot, booking.BookingId);

            return await Task.FromResult(booking);
        }
    }
}
