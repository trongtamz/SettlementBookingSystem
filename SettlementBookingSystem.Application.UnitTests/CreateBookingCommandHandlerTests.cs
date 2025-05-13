using FluentAssertions;
using FluentValidation;
using Moq;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CreateBookingCommandHandlerTests
    {
        [Fact]
        public async Task GivenValidBookingTime_WhenNoConflictingBookings_ThenBookingIsAccepted()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "09:15",
            };

            var bookingRepoMock = new Mock<IBookingRepository>();
            bookingRepoMock.Setup(r => r.GetBookingCount("09:15")).Returns(0);

            var handler = new CreateBookingCommandHandler(bookingRepoMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.BookingId.Should().NotBeEmpty();
        }

        [Fact]
        public void GivenOutOfHoursBookingTime_WhenBooking_ThenValidationFails()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "00:00",
            };

            var bookingRepoMock = new Mock<IBookingRepository>();
            var handler = new CreateBookingCommandHandler(bookingRepoMock.Object);

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void GivenValidBookingTime_WhenBookingIsFull_ThenConflictThrown()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "09:15",
            };

            var bookingRepoMock = new Mock<IBookingRepository>();
            bookingRepoMock.Setup(r => r.GetBookingCount("09:15")).Returns(4);

            var handler = new CreateBookingCommandHandler(bookingRepoMock.Object);

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            act.Should().Throw<ConflictException>();
        }
    }
}
