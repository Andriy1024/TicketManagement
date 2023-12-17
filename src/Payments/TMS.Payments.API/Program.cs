using TMS.Common.Extensions;

using TMS.Payments.API;
using TMS.Payments.Persistence;
using TMS.Payments.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddPeristence(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddProblemDetails();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseMiddleware<ErrorMiddleware>();

app.UseAuthorization();

app.MapControllers();

await app.Services.RunStartupTasksAsync();

await app.RunAsync();