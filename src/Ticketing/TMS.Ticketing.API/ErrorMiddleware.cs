using TMS.Common.Errors;

namespace TMS.Ticketing.API;

internal class ErrorMiddleware
{
    private readonly RequestDelegate next;

    public ErrorMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
		try
		{
			await next(context);
		}
		catch (Exception e)
		{
			var error = e switch
			{
				AppException app => app.Error,
				_ => AppError.InternalServerError(e.Message)
			};

			context.Response.StatusCode = (int)error.StatusCode;

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
