using DfdsTestTask.Exceptions;
using DfdsTestTask.Features.BookingManagement.BusinessLogic.Interfaces;
using DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;
using DfdsTestTask.PersistenceShared;
using DfdsTestTask.PersistenceShared.Entities;
using Microsoft.EntityFrameworkCore;

namespace DfdsTestTask.Features.BookingManagement.Persistence;

public class BookingRepository(
    BookingManagementDbContext managementDbContext,
    ILogger<BookingRepository> logger
): IBookingRepository
{
    public async Task CreateBooking(BookingModel bookingModel, CancellationToken ct = default)
    {
        try
        {
            var enumeratedUserIds = bookingModel.AssignedUsers.Select(it => it.Value);
            var fetchedUserEntities = await managementDbContext.Users
                .AsNoTracking()
                .Where(it => enumeratedUserIds.Contains(it.Id))
                .ToListAsync(cancellationToken: ct);
            managementDbContext.Bookings.Add(new BookingEntity
            {
                AggregateVersion = bookingModel.AggregateVersion,
                Date = bookingModel.BookingDate,
                Users = fetchedUserEntities
            });

            await managementDbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, ex.StackTrace);
            throw new PersistenceOperationFailedException(
                "Failed to store created a booking"
                );
        }
        
    }

    public async Task DeleteBooking(BookingId bookingId, CancellationToken ct)
    {
        var targetBooking = await managementDbContext.Bookings.SingleOrDefaultAsync(
            booking => booking.Id == bookingId.Value, 
            cancellationToken: ct
            );
        if (targetBooking != null)
        {
            managementDbContext.Bookings.Remove(targetBooking);
        }
    }

    public async Task<IEnumerable<BookingModel>> ListBookings(CancellationToken ct)
    {
        try
        {
            var fetchedBookings = await managementDbContext.Bookings
                .AsNoTracking()
                .Include(it => it.Users)
                .ToListAsync(cancellationToken: ct);
            return fetchedBookings.Select(ProjectModel);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, ex.StackTrace);
            throw new PersistenceOperationFailedException(
                "Failed to store created a booking"
            );
        }
    }

    public async Task<BookingModel?> LoadBookingById(BookingId bookingId, CancellationToken ct)
    {
        var targetBooking = await managementDbContext.Bookings
            .Include(it => it.Users)
            .AsNoTracking()
            .SingleOrDefaultAsync(it => it.Id == bookingId.Value, cancellationToken: ct);

        if (targetBooking == null)
        {
            return null;
        }

        return ProjectModel(targetBooking);
    }

    public async Task UpdateBooking(BookingModel matchingBookingModel, CancellationToken ct)
    {
        try
        {
            var enumeratedIds = matchingBookingModel.AssignedUsers.Select(it => it.Value);
            var fetchedUsers = await managementDbContext.Users
                .Where(it => enumeratedIds.Contains(it.Id))
                .AsNoTracking()
                .ToListAsync(cancellationToken: ct);

            var entity = new BookingEntity
            {
                Id = matchingBookingModel.Id.Value,
                Date = matchingBookingModel.BookingDate,
                Users = fetchedUsers
            };

            managementDbContext.Attach(entity);
            managementDbContext.Entry(entity).State = EntityState.Modified;
            await managementDbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, ex.StackTrace);
            throw new PersistenceOperationFailedException(
                "Failed to load  a booking"
            );
        }
    }

    private BookingModel ProjectModel(BookingEntity bookingEntity)
        => new BookingModel
        {
            AggregateVersion = bookingEntity.AggregateVersion,
            AssignedUsers = bookingEntity.Users.Select(user => UserId.FromValue(user.Id)),
            Id = BookingId.FromValue(bookingEntity.Id),
            BookingDate = bookingEntity.Date
        };
}