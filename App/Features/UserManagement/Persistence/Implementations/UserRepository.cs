using DfdsTestTask.Exceptions;
using DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;
using DfdsTestTask.Features.UserManagement.Persistence.Interfaces;
using DfdsTestTask.PersistenceShared;
using DfdsTestTask.PersistenceShared.Entities;
using Microsoft.EntityFrameworkCore;

namespace DfdsTestTask.Features.UserManagement.Persistence.Implementations;

public class UserRepository(
    BookingManagementDbContext bookingManagementDbContext,
    ILogger<UserRepository> logger
) : IUserRepository
{
    
    
    public async Task CreateUser(UserModel userModel, CancellationToken ct)
    {
        try
        {
            var userCreationEntity = new UserEntity
            {
                PassportNumber = userModel.EncryptedPassportNumber
            };

            bookingManagementDbContext.Users.Add(userCreationEntity);
            await bookingManagementDbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, ex.StackTrace);
            throw new PersistenceOperationFailedException(
                "Failed to create a user"
                );
        }
    }

    public async Task<UserModel?> LoadUserById(UserId userId, CancellationToken ct = default)
    {
        try
        {
            var targetUserEntity = await bookingManagementDbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    user => user.Id == userId.Value, 
                    cancellationToken: ct
                    );
            
            if (targetUserEntity == null)
            {
                return null;
            }

            return ProjectUser(targetUserEntity);

        }catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, ex.StackTrace);
            throw new PersistenceOperationFailedException(
                "Failed to load user by id"
            );
        }
    }


    public async Task<IEnumerable<UserModel>> ListUsers(CancellationToken ct = default)
    {
        try
        { 
            var storedUsers 
                = await bookingManagementDbContext.Users
                    .AsNoTracking()
                    .ToListAsync(ct);
            return storedUsers.Select(ProjectUser);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, ex.StackTrace);
            throw new PersistenceOperationFailedException(
                "Failed to list users"
            );
        }
    }

    public async Task<IEnumerable<UserModel>> ListUsersByBookingId(
        BookingId bookingId, 
        CancellationToken ct = default
    )
    {
        try
        {
            var targetBooking = await bookingManagementDbContext.Bookings
                .Include(booking => booking.Users)
                .AsNoTracking()
                .SingleOrDefaultAsync(booking => booking.Id == bookingId.Value, cancellationToken: ct);

            if (targetBooking == null)
            {
                return Enumerable.Empty<UserModel>();
            }

            return targetBooking.Users.Select(ProjectUser);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, ex.StackTrace);
            throw new PersistenceOperationFailedException(
                "Failed to list users by booking id"
            );
        }
    }

    public async Task DeleteUserById(UserId bookingId, CancellationToken ct = default)
    {
        try
        {
            var targetUser = await bookingManagementDbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    user => user.Id == bookingId.Value, 
                    cancellationToken: ct
                );

            if (targetUser == null)
            {
                return;
            }

            bookingManagementDbContext.Users.Remove(targetUser);
            await bookingManagementDbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, ex.StackTrace);
            throw new PersistenceOperationFailedException(
                "Failed to delete user by id"
            );
        }
    }

    public async Task UpdateUser(UserModel userModel, CancellationToken ct = default)
    {
        try
        {
            var entity = new UserEntity
            {
                Id = userModel.Id.Value,
                PassportNumber = userModel.EncryptedPassportNumber,
                Version = userModel.AggregateVersion
            };

            bookingManagementDbContext.Attach(entity);
        
            bookingManagementDbContext.Entry(entity).State = EntityState.Modified;

            await bookingManagementDbContext.SaveChangesAsync(ct);
           
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message, ex.StackTrace);
            throw new PersistenceOperationFailedException(
                "Failed to update user"
            );
        }
    }

    private UserModel ProjectUser(UserEntity user)
        => new UserModel
        {
            Id = UserId.FromValue(user.Id),
            AggregateVersion = user.Version,
            EncryptedPassportNumber = user.PassportNumber,
        };
}