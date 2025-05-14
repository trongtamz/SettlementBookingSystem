using FluentAssertions;
using FluentValidation;
using Moq;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Exceptions;
using System;
using System.Collections.Generic;
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
            bookingRepoMock.Setup(r => r.GetAllBookings()).Returns(new Dictionary<string, IEnumerable<Guid>>());

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

            var existingBookings = new Dictionary<string, IEnumerable<Guid>>
            {
                { "09:00", new[] { Guid.NewGuid() } },
                { "09:10", new[] { Guid.NewGuid() } },
                { "09:20", new[] { Guid.NewGuid() } },
                { "09:30", new[] { Guid.NewGuid() } }
            };

            var bookingRepoMock = new Mock<IBookingRepository>();
            bookingRepoMock.Setup(r => r.GetAllBookings()).Returns(existingBookings);

            var handler = new CreateBookingCommandHandler(bookingRepoMock.Object);

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            act.Should().Throw<ConflictException>();
        }
    }
}