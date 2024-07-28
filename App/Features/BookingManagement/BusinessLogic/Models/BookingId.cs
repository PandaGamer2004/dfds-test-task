using DfdsTestTask.Features.Shared.Models;

namespace DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;

public class BookingId: EntityID
{
    public static BookingId FromValue(int id)
        => FromValue(new BookingId(), id, typeof(BookingModel));
}