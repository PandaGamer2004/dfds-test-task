namespace DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;

public class BookingCreationModel
{
    public DateTime AtDate { get; set; }

    public IEnumerable<int> InitialUsers { get; set; }
}