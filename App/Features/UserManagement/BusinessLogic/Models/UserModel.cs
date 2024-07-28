using DfdsTestTask.Features.Shared.Models;

namespace DfdsTestTask.Features.UserManagement.BusinessLogic.Models;

public class UserModel: VersionedAggregate
{
    public UserId Id { get; set; }

    public string PassportDetail { get; set; }
}