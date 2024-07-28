using DfdsTestTask.Exceptions;
using DfdsTestTask.Features.Encryption.Interfaces;
using DfdsTestTask.Features.Encryption.Models;
using DfdsTestTask.Features.Shared.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Interfaces;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;
using DfdsTestTask.Features.UserManagement.Persistence.Interfaces;
using DfdsTestTask.PersistenceShared;

namespace DfdsTestTask.Features.UserManagement.BusinessLogic.Implementations;

public class UserService(
    IEncryptionConfigurationLoader<SymmetricEncryptionContext> encryptionConfigurationLoader,
    ISymmetricStringDataEncryptor symmetricStringDataEncryptor,
    IUserRepository userRepository
): IUserService
{

    
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
        catch (PersistenceOperationFailedException ex)
        {
            return BusinessOperationResult<VoidResult, string>.CreateError(
                "Failed to store user data"
            );
        }
    }

    public Task<BusinessOperationResult<IEnumerable<UserOutboundModel>, string>> ListUsersForBooking(int id)
    {
        throw new NotImplementedException();
    }

    public Task<BusinessOperationResult<IEnumerable<UserOutboundModel>, string>> ListUsers(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<BusinessOperationResult<VoidResult, string>> DeleteUser(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<BusinessOperationResult<VoidResult, string>> UpdateUser(UserOutboundModel userModel)
    {
        throw new NotImplementedException();
    }
}