namespace DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;

public class BookingOutboundModel
{
    public int Id { get; set; }
    

    public DateTime AtDate { get; set; }
    
    public IEnumerable<int> UserIds { get; set; }
}