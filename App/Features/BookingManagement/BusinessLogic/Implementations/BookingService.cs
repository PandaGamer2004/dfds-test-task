using DfdsTestTask.Exceptions;
using DfdsTestTask.Extensions;
using DfdsTestTask.Features.BookingManagement.BusinessLogic.Interfaces;
using DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;
using DfdsTestTask.Features.Shared.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Interfaces;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;

namespace DfdsTestTask.Features.BookingManagement.BusinessLogic.Implementations;

public class BookingService(
    IBookingRepository bookingRepository,
    IUserService userService
): IBookingService
{
    public async Task<BusinessOperationResult<VoidResult, string>> CreateBooking(
        BookingCreationModel bookingCreationModel, 
        CancellationToken ct = default
    )
    {
        try
        {
            var projectedUserIds = bookingCreationModel.InitialUsers.Select(UserId.FromValue).ToList();

            return await userService.ValidateUserIds(projectedUserIds, ct)
                .UnwrapAsync(fetchResult =>
                {
                    return fetchResult.FlatMapAsync(async validationResult =>
                    {
                        if (validationResult.IsValid)
                        {
                            var coreModel = new BookingModel
                            {
                                AggregateVersion = 1,
                                AssignedUsers = projectedUserIds,
                                BookingDate = bookingCreationModel.AtDate,
                            };
                            try
                            {
                                await bookingRepository.CreateBooking(coreModel, ct);
                                return BusinessOperationResult<VoidResult, string>.CreateSuccess(
                                    VoidResult.Instance
                                );
                            }
                            catch (PersistenceOperationFailedException ex)
                            {
                                return BusinessOperationResult<VoidResult, string>.CreateError(
                                    "Failed to store booking");
                            }
                        }

                        return BusinessOperationResult<VoidResult, string>.CreateError(
                            "Invalid user ids was passed"
                        );
                    });
                }).Unwrap();
        }
        catch (EntityInitializationException)
        {
            return BusinessOperationResult<VoidResult, string>.CreateError(
                "Invalid user id's passed as default for a booking"
            );
        }
    }

    public async Task<BusinessOperationResult<VoidResult, string>> DeleteBooking(int bookingId, CancellationToken ct = default)
    {
        try
        {
            await bookingRepository.DeleteBooking(BookingId.FromValue(bookingId), ct);
            return BusinessOperationResult<VoidResult, string>.CreateSuccess(
                VoidResult.Instance
            );
        }
        catch (PersistenceOperationFailedException)
        {
            return BusinessOperationResult<VoidResult, string>.CreateError(
                "Failed to delete requested booking"
            );
        }
        catch (EntityInitializationException)
        {
            return BusinessOperationResult<VoidResult, string>.CreateError(
                "Invalid user id's passed for a booking"
            );
        }
    }

    public async Task<BusinessOperationResult<VoidResult, string>> UpdateBooking(BookingOutboundModel bookingOutboundModel, CancellationToken ct = default)
    {
        try
        {
            var matchingBookingModel = await bookingRepository
                    .LoadBookingById(BookingId.FromValue(bookingOutboundModel.Id), ct);


            if (matchingBookingModel == null)
            {
                return BusinessOperationResult<VoidResult, string>.CreateError(
                    "Unable to find stored booking associated with passed id"
                );
            }
            var projectedUserIds = bookingOutboundModel.UserIds.Select(UserId.FromValue).ToList();
            
            return await userService.ValidateUserIds(projectedUserIds, ct)
                .UnwrapAsync(fetchResult =>
                {
                    return fetchResult.FlatMapAsync(async validationResult =>
                    {
                        if (validationResult.IsValid)
                        {
                            matchingBookingModel.AggregateVersion += 1;
                            matchingBookingModel.AssignedUsers = bookingOutboundModel.UserIds.Select(UserId.FromValue);
                            matchingBookingModel.BookingDate = bookingOutboundModel.AtDate;
                            try
                            {
                                await bookingRepository.UpdateBooking(matchingBookingModel, ct);
                                return BusinessOperationResult<VoidResult, string>.CreateSuccess(
                                    VoidResult.Instance
                                );
                            }
                            catch (PersistenceOperationFailedException ex)
                            {
                                return BusinessOperationResult<VoidResult, string>.CreateError(
                                    "Failed to update booking");
                            }
                        }

                        return BusinessOperationResult<VoidResult, string>.CreateError(
                            "Invalid user ids was passed"
                        );
                    });
                }).Unwrap();
        }
        catch (EntityInitializationException ex)
        {
            if (ex.OnTypeToInitialize == typeof(BookingId))
            {
                return BusinessOperationResult<VoidResult, string>.CreateError(
                    "Invalid booking id passed for booking update"
                );    
            }

            return BusinessOperationResult<VoidResult, string>.CreateError(
                "Invalid user ids was passed for booking update"
            );
        }
    }

    public async Task<BusinessOperationResult<IEnumerable<BookingOutboundModel>, string>> ListBookings(CancellationToken ct = default)
    {
        try
        {
            var responseBookings = await bookingRepository.ListBookings(ct);
            var projectedBookings = responseBookings.Select(booking => new BookingOutboundModel
            {
                Id = booking.Id.Value,
                AtDate = booking.BookingDate,
                UserIds = booking.AssignedUsers.Select(it => it.Value)
            });
            return BusinessOperationResult<IEnumerable<BookingOutboundModel>, string>.CreateSuccess(
                projectedBookings
                );
        }
        catch (PersistenceOperationFailedException)
        {
            return BusinessOperationResult<IEnumerable<BookingOutboundModel>, string>.CreateError(
                "Failed to list bookings"
            );
        }
    }
}