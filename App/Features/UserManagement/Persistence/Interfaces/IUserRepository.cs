using DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;

namespace DfdsTestTask.Features.UserManagement.Persistence.Interfaces;

public interface IUserRepository
{
    public Task CreateUser(UserModel userModel, CancellationToken ct = default);

    public Task<UserModel?> LoadUserById(UserId userId, CancellationToken ct = default);
    
    public Task<IEnumerable<UserModel>> ListUsers(CancellationToken ct = default);

    public Task<IEnumerable<UserModel>> ListUsersByBookingId(BookingId bookingId, CancellationToken ct = default);

    public Task DeleteUserById(UserId userId, CancellationToken ct = default);

    public Task UpdateUser(UserModel userModel, CancellationToken ct = default);

}