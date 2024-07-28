using DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;
using DfdsTestTask.Features.Shared.Models;

namespace DfdsTestTask.Features.BookingManagement.BusinessLogic.Interfaces;

public interface IBookingService
{
    Task<BusinessOperationResult<VoidResult, string>> CreateBooking(
        BookingCreationModel bookingCreationModel, 
        CancellationToken ct = default
    );

    Task<BusinessOperationResult<VoidResult, string>> DeleteBooking(
        int bookingId,
        CancellationToken ct = default
    );

    Task<BusinessOperationResult<VoidResult, string>> UpdateBooking(
        BookingOutboundModel bookingOutboundModel,
        CancellationToken ct = default
    );

    Task<BusinessOperationResult<IEnumerable<BookingOutboundModel>, string>> ListBookings(
        CancellationToken ct = default
        );
}