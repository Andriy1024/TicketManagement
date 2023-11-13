using System.Net;

namespace TMS.Common.Errors;

public class AppError
{
    public string Message { get; init; }

    public HttpStatusCode StatusCode { get; init; }

    public AppError(string message, HttpStatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public static AppError NotFound(string message)
        => new(message, HttpStatusCode.NotFound);

    public static AppError InvalidData(string message)
       => new(message, HttpStatusCode.BadRequest);

    public static AppError Forbidden(string message)
       => new(message, HttpStatusCode.Forbidden);

    public static AppError InternalServerError(string message)
       => new(message, HttpStatusCode.InternalServerError);

    public AppException ToException() => new(this);
}