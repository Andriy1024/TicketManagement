namespace TMS.Common.Errors;

public class AppException : Exception
{
    public AppError Error { get; }

    public AppException(AppError error)
    {
        Error = error;
    }
}