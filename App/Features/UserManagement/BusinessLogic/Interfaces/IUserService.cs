using DfdsTestTask.Features.Shared.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;

namespace DfdsTestTask.Features.UserManagement.BusinessLogic.Interfaces;

public interface IUserService
{

    Task<BusinessOperationResult<UserIdsValidationResult, string>> ValidateUserIds(
        IEnumerable<UserId> userIds, 
        CancellationToken ct = default
        );
    Task<BusinessOperationResult<VoidResult, string>> CreateUser(
        UserCreationModel userCreationModel,
        CancellationToken ct = default
        );


    Task<BusinessOperationResult<IEnumerable<UserOutboundModel>, string>> ListUsersForBooking(
        int bookingId, 
        CancellationToken ct = default
        );
    
    Task<BusinessOperationResult<IEnumerable<UserOutboundModel>, string>> ListUsers(
        CancellationToken ct = default
        );

    Task<BusinessOperationResult<VoidResult, string>> DeleteUser(
        int userId,
        CancellationToken ct = default
        );
    
    Task<BusinessOperationResult<VoidResult, string>> UpdateUser(
        UserOutboundModel userModel,
        CancellationToken ct = default);
}