using DfdsTestTask.Features.Shared.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;

namespace DfdsTestTask.Features.UserManagement.BusinessLogic.Interfaces;

public interface IUserService
{
    Task<BusinessOperationResult<VoidResult, string>> CreateUser(
        UserCreationModel userCreationModel,
        CancellationToken ct = default
        );


    Task<BusinessOperationResult<IEnumerable<UserOutboundModel>, string>> ListUsersForBooking(int id);
    
    Task<BusinessOperationResult<IEnumerable<UserOutboundModel>, string>> ListUsers(
        CancellationToken ct = default
        );

    Task<BusinessOperationResult<VoidResult, string>> DeleteUser(int userId);
    
    Task<BusinessOperationResult<VoidResult, string>> UpdateUser(UserOutboundModel userModel);
}