namespace DfdsTestTask.Features.Shared.Models;

public class BusinessOperationResult<TSuccess, TError>
{
    public bool IsSuccess { get; set; }

    public TSuccess Success { get; set; }

    public TError Error { get; set; }

    public static BusinessOperationResult<TSuccess, TError> CreateSuccess(TSuccess success)
        => new BusinessOperationResult<TSuccess, TError>
        {
            IsSuccess = true,
            Success = success
        };

    public static BusinessOperationResult<TSuccess, TError> CreateError(TError error)
        => new BusinessOperationResult<TSuccess, TError>
        {
            IsSuccess = false,
            Error = error
        };
}