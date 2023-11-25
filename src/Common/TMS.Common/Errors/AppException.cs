namespace TMS.Common.Errors;

public class AppException : Exception
{
    public ApiError Error { get; }

    public AppException(ApiError error)
    {
        Error = error;
    }
}