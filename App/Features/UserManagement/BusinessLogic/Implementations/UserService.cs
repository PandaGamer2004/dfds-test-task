using DfdsTestTask.Exceptions;
using DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;
using DfdsTestTask.Features.Encryption.Interfaces;
using DfdsTestTask.Features.Encryption.Models;
using DfdsTestTask.Features.Shared.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Interfaces;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;

namespace DfdsTestTask.Features.UserManagement.BusinessLogic.Implementations;

public class UserService(
    IEncryptionConfigurationLoader<SymmetricEncryptionContext> encryptionConfigurationLoader,
    ISymmetricStringDataEncryptor symmetricStringDataEncryptor,
    IUserRepository userRepository
): IUserService
{
    public async Task<BusinessOperationResult<UserIdsValidationResult, string>> ValidateUserIds(
        IEnumerable<UserId> userIds, 
        CancellationToken ct
    )
    {
        try
        {
            var enumeratedUserIds = userIds.ToList();
            int userIdsCount = await userRepository.LoadMatchingUsersCount(enumeratedUserIds, ct);
            
            return BusinessOperationResult<UserIdsValidationResult, string>.CreateSuccess(
                new UserIdsValidationResult
                {
                    IsValid = userIdsCount == enumeratedUserIds.Count
                });
        }
        catch (PersistenceOperationFailedException)
        {
            return BusinessOperationResult<UserIdsValidationResult, string>.CreateError(
                "Failed to load users"
            );
        }
    }

    public async Task<BusinessOperationResult<VoidResult, string>> CreateUser(UserCreationModel userCreationModel, CancellationToken ct = default)
    {
        try
        {
            var symmetricEncryptionConfiguration
                = await encryptionConfigurationLoader.LoadConfiguration(ct);

            var encryptedPassportNumber = symmetricStringDataEncryptor.Encrypt(
                userCreationModel.PassportNumber,
                symmetricEncryptionConfiguration
            );

            var userModel = new UserModel
            {
                EncryptedPassportNumber = encryptedPassportNumber
            };

            await userRepository.CreateUser(userModel, ct);
            
            return BusinessOperationResult<VoidResult, string>.CreateSuccess(
                VoidResult.Instance
            );
        }
        catch (PersistenceOperationFailedException)
        {
            return BusinessOperationResult<VoidResult, string>.CreateError(
                "Failed to store user data"
            );
        }
    }

    public async Task<BusinessOperationResult<IEnumerable<UserOutboundModel>, string>> ListUsersForBooking(
        int bookingId, 
        CancellationToken ct = default
        )
    {
        try
        {
            var projectedBookingId = BookingId.FromValue(bookingId);
            var userModels = await userRepository.ListUsersByBookingId(projectedBookingId, ct);
  
            var userModelProjector = await CreateModelProjector(ct);
            return BusinessOperationResult<IEnumerable<UserOutboundModel>, string>.CreateSuccess(
                userModels.Select(userModelProjector)
                );
        }
        catch (EntityInitializationException)
        {
            return BusinessOperationResult<IEnumerable<UserOutboundModel>, string>.CreateError(
                "Invalid booking id was supplied"
            );
        }
        catch (PersistenceOperationFailedException)
        {
            return BusinessOperationResult<IEnumerable<UserOutboundModel>, string>.CreateError(
                "Failed to access users from storage"
                );
        }
    }

    public async Task<BusinessOperationResult<IEnumerable<UserOutboundModel>, string>> ListUsers(CancellationToken ct = default)
    {
        try
        {
            var usersList = await userRepository.ListUsers(ct);
            var userModelProjector = await CreateModelProjector(ct);
            var projectedUsers = usersList.Select(userModelProjector);
            return BusinessOperationResult<IEnumerable<UserOutboundModel>, string>.CreateSuccess(
                projectedUsers
            );
        }
        catch (PersistenceOperationFailedException)
        {
            return BusinessOperationResult<IEnumerable<UserOutboundModel>, string>.CreateError(
                "Failed to access users from storage"
            );
        }
    }

    public async Task<BusinessOperationResult<VoidResult, string>> DeleteUser(
        int userId,
        CancellationToken ct = default
    )
    {
        try
        {
            var projectedUserId = UserId.FromValue(userId);
            await userRepository.DeleteUserById(projectedUserId, ct);
            return BusinessOperationResult<VoidResult, string>.CreateSuccess(
                VoidResult.Instance
            );
        }
        catch (PersistenceOperationFailedException)
        {
            return BusinessOperationResult<VoidResult, string>.CreateError(
                "Failed to delete user from storage"
            );
        }
        catch (EntityInitializationException)
        { 
            return BusinessOperationResult<VoidResult, string>.CreateError(
                "Invalid user id was supplied"
            );
        }
    }

    public async Task<BusinessOperationResult<VoidResult, string>> UpdateUser(
        UserOutboundModel userModel,
        CancellationToken ct = default
    )
    {
        try
        {
            var resultId = UserId.FromValue(userModel.Id);
            var targetUser = await userRepository.LoadUserById(resultId, ct);
            if (targetUser == null)
            {
                return BusinessOperationResult<VoidResult, string>.CreateError(
                    "Failed to update model that not exits"
                );
            }

            var symmetricEncryptionContext 
                = await encryptionConfigurationLoader.LoadConfiguration(ct);
            var userModelToUpdate = new UserModel
            {
                AggregateVersion = targetUser.AggregateVersion,
                Id = targetUser.Id,
                EncryptedPassportNumber =
                    symmetricStringDataEncryptor.Encrypt(userModel.PassportNumber, symmetricEncryptionContext),
            };
            
            await userRepository.UpdateUser(userModelToUpdate, ct);

            return BusinessOperationResult<VoidResult, string>
                .CreateSuccess(VoidResult.Instance);
        }
        catch (PersistenceOperationFailedException)
        {
            return BusinessOperationResult<VoidResult, string>.CreateError(
                "Failed to update user from storage"
            );
        }
        catch (EntityInitializationException)
        {
            return BusinessOperationResult<VoidResult, string>.CreateError(
                "Invalid user id was supplied"
            );
        }
    }

    private async Task<Func<UserModel, UserOutboundModel>> CreateModelProjector(CancellationToken ct)
    {
        var encryptionContext = await encryptionConfigurationLoader.LoadConfiguration(ct);
        return (userModel) =>
        {
            var decryptedPassport 
                = symmetricStringDataEncryptor.Decrypt(userModel.EncryptedPassportNumber, encryptionContext);
            return new UserOutboundModel
            {
                Id = userModel.Id.Value,
                PassportNumber = decryptedPassport,
            };
        };
    }
}