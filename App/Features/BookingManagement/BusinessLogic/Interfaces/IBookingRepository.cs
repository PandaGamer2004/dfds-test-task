using DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;

namespace DfdsTestTask.Features.BookingManagement.BusinessLogic.Interfaces;

public interface IBookingRepository
{
    Task CreateBooking(BookingModel bookingModel, CancellationToken ct = default);
    Task DeleteBooking(BookingId bookingId, CancellationToken ct);
    Task<IEnumerable<BookingModel>> ListBookings(CancellationToken ct);
    Task<BookingModel?> LoadBookingById(BookingId bookingId, CancellationToken ct);
    Task UpdateBooking(BookingModel matchingBookingModel, CancellationToken ct);
}