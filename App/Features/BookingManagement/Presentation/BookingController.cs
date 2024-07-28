using DfdsTestTask.Extensions;
using DfdsTestTask.Features.BookingManagement.BusinessLogic.Interfaces;
using DfdsTestTask.Features.BookingManagement.BusinessLogic.Models;
using DfdsTestTask.Features.Shared.Presentation;
using Microsoft.AspNetCore.Mvc;

namespace DfdsTestTask.Features.BookingManagement.Presentation;

public class BookingController(IBookingService bookingService): BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody]BookingCreationModel creationModel)
    {
        var result = await bookingService.CreateBooking(creationModel)
            .UnwrapAsync(ResponseUtilities.ProjectResponse);
        return result.Success;
    }


    [HttpDelete("{bookingId:int}")]
    public async Task<IActionResult> DeleteBookingById([FromRoute] int bookingId)
    {
        var result = await bookingService.DeleteBooking(bookingId)
            .UnwrapAsync(ResponseUtilities.ProjectResponse);
        return result.Success;
    }


    [HttpPut]
    public async Task<IActionResult> UpdateBooking([FromBody] BookingOutboundModel updateModel)
    {
        var result = await bookingService.UpdateBooking(updateModel)
            .UnwrapAsync(ResponseUtilities.ProjectResponse);
        return result.Success;
    }

    [HttpGet]
    public async Task<IActionResult> ListBookings()
    {
        var result = await bookingService.ListBookings()
            .UnwrapAsync(ResponseUtilities.ProjectResponse);
        return result.Success;
    }
}