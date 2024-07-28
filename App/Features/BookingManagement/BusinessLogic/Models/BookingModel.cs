using DfdsTestTask.Features.Shared;
using DfdsTestTask.Features.Shared.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;

namespace DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;

public class BookingModel: VersionedAggregate
{
    public int Id { get; set; }
    
    public DateTime BookingDate { get; set; }

    public IEnumerable<UserId> AssignedUsers { get; set; }
}