using System.Net;

namespace TMS.Common.Errors;

public class ApiError
{
    public string Message { get; init; }

    public HttpStatusCode StatusCode { get; init; }

    public ApiError(string message, HttpStatusCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }

    public static ApiError NotFound(string message)
        => new(message, HttpStatusCode.NotFound);

    public static ApiError InvalidData(string message)
       => new(message, HttpStatusCode.BadRequest);

    public static ApiError Forbidden(string message)
       => new(message, HttpStatusCode.Forbidden);

    public static ApiError InternalServerError(string message)
       => new(message, HttpStatusCode.InternalServerError);

    public AppException ToException() => new(this);
}