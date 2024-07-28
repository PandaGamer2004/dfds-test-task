using DfdsTestTask.Features.Shared.Models;

namespace DfdsTestTask.Features.UserManagement.BusinessLogic.Models;

public class UserId: EntityID
{
    public static UserId FromValue(int userId)
        => FromValue(new UserId(), userId, typeof(UserModel));
    
}