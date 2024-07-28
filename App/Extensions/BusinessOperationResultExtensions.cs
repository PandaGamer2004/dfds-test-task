using DfdsTestTask.Features.Shared.Models;

namespace DfdsTestTask.Extensions;

public static class BusinessOperationResultExtensions
{
    public static async Task<BusinessOperationResult<TResult2, TError2>> UnwrapAsync<TResult, TResult2, TError, TError2>(
        this Task<BusinessOperationResult<TResult, TError>> task,
        Func<BusinessOperationResult<TResult, TError>, BusinessOperationResult<TResult2, TError2>> projector)
    {
        var result = await task;
        return projector(result);
    }

    public static BusinessOperationResult<TResult, NoError> ProjectError<TResult,  TError>(
        this BusinessOperationResult<TResult, TError> result,
        Func<TError, TResult> projector
    )
    {
        if (!result.IsSuccess)
        {
            return BusinessOperationResult<TResult, NoError>.CreateSuccess(
                    projector(result.Error)
                );
        }
        return BusinessOperationResult<TResult, NoError>.CreateSuccess(
                result.Success
            );
        
    }
    

    public static BusinessOperationResult<TResult2, TError> Map<TResult, TResult2, TError>(
        this BusinessOperationResult<TResult, TError> result,
        Func<TResult, TResult2> projector)
    {
        return result.FlatMap(res
            => BusinessOperationResult<TResult2, TError>.CreateSuccess(projector(res))
            );
    }

    public static BusinessOperationResult<TResult2, TError> FlatMap<TResult, TResult2, TError>(
        this BusinessOperationResult<TResult, TError> result,
        Func<TResult, BusinessOperationResult<TResult2, TError>> projector)
    {
        if (result.IsSuccess)
        {
            return projector(result.Success);
        }
        return BusinessOperationResult<TResult2, TError>.CreateError(
            result.Error
            );
    }
}