using DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;
using DfdsTestTask.Features.UserManagement.Persistence.Interfaces;

namespace DfdsTestTask.Features.UserManagement.Persistence.Implementations;

public class UserRepository: IUserRepository
{
    public Task CreateUser(UserModel userModel, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> LoadUserById(UserId userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> ListUsers(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> ListUsersByBookingId(BookingId bookingId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserId(UserId bookingId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUser(UserModel userModel, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}