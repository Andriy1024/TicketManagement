namespace TMS.Common.Errors;

public class ApiException : Exception
{
    public ApiError Error { get; }

    public ApiException(ApiError error)
    {
        Error = error;
    }
}