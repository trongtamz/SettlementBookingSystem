using FluentValidation;
using System;
using System.Globalization;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.BookingTime)
                .Matches("^(?:[01][0-9]|2[0-3]):[0-5][0-9]$")
                .Must(time =>
                {
                    if (!TimeSpan.TryParseExact(time?.Trim(), "hh\\:mm", CultureInfo.InvariantCulture, out var ts))
                        return false;
                    return ts >= new TimeSpan(9, 0, 0) && ts <= new TimeSpan(16, 0, 0);
                })
                .WithMessage("BookingTime must be between 09:00 and 16:00 and in HH:mm 24-hour format.");
        }
    }
}
