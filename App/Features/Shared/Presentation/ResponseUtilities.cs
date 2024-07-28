using DfdsTestTask.Extensions;
using DfdsTestTask.Features.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DfdsTestTask.Features.Shared.Presentation;

public class ResponseUtilities
{
    public static BusinessOperationResult<IActionResult, NoError> ProjectResponse<TResult>(
        BusinessOperationResult<TResult, string> operationResult
    )
        => operationResult.Map(res =>
            {
                if (res is VoidResult)
                {
                    return (IActionResult)new OkResult();
                }

                return new JsonResult(res) as IActionResult;
            })
            .ProjectError(err => (IActionResult)new BadRequestObjectResult(err));
}