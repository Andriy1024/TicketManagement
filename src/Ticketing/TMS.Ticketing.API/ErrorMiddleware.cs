using TMS.Common.Errors;

namespace TMS.Ticketing.API;

internal class ErrorMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IProblemDetailsService _problemDetailsService;

    public ErrorMiddleware(RequestDelegate next, IProblemDetailsService problemDetailsService)
    {
        _next = next;
        _problemDetailsService = problemDetailsService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
		try
		{
			await _next(context);
		}
		catch (Exception e)
		{
			var error = e switch
			{
				ApiException app => app.Error,
				_ => ApiError.InternalServerError(e.Message)
            };

            context.Response.StatusCode = (int)error.StatusCode;

            var problems = new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails =
                {
                    Title = error.Message,
                    Detail = e.Message,
                    Type = error.GetType().Name,
                    Status = (int)error.StatusCode
                }
            };

            problems.ProblemDetails.Extensions.Add("StackTrace", e.StackTrace);

            await _problemDetailsService.WriteAsync(problems);
        }
    }
}
