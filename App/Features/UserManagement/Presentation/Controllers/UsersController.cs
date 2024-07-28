using DfdsTestTask.Extensions;
using DfdsTestTask.Features.Shared.Models;
using DfdsTestTask.Features.Shared.Presentation;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Interfaces;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace DfdsTestTask.Features.UserManagement.Presentation.Controllers;

public class UsersController(
    IUserService userService
): BaseApiController
{

    [HttpGet]
    public async Task<IActionResult> ListUsers()
    {
        var usersListResult
            = await userService.ListUsers(CancellationToken.None)
                .UnwrapAsync(ResponseUtilities.ProjectResponse);
        return usersListResult.Success;
    }

    [HttpGet("byBooking/{bookingId:int}")]
    public async Task<IActionResult> ListUsersById([FromRoute]int bookingId)
    {
        var listByBookingResult = await userService.ListUsersForBooking(bookingId)
            .UnwrapAsync(ResponseUtilities.ProjectResponse);
        return listByBookingResult.Success;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody]UserCreationModel userCreationModel)
    {
        var userCreationResult = await userService.CreateUser(userCreationModel)
            .UnwrapAsync(ResponseUtilities.ProjectResponse);
        return userCreationResult.Success;
    }

    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute]int userId)
    {
        var userDeletionResult = await userService.DeleteUser(userId)
            .UnwrapAsync(ResponseUtilities.ProjectResponse);
        return userDeletionResult.Success;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody]UserOutboundModel userModel)
    {
        var userUpdateResult = await userService.UpdateUser(userModel)
            .UnwrapAsync(ResponseUtilities.ProjectResponse);
        return userUpdateResult.Success;
    }

    

    
}