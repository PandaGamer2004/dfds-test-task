using DfdsTestTask.Features.Shared.Models;

namespace DfdsTestTask.Features.UserManagement.BusinessLogic.Models;

//For current aggregate we will use default entity framework optimistic lock
public class UserModel: VersionedAggregate<byte[]>
{
    public UserId Id { get; set; }

    public string EncryptedPassportNumber { get; set; }
}