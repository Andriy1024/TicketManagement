using TMS.Common.Extensions;

using TMS.Ticketing.API;
using TMS.Ticketing.Persistence;
using TMS.Ticketing.Infrastructure;
using TMS.Observability;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddInfrastructure(builder.Configuration)
    .AddPersistenceServices(builder.Configuration)
    .AddProblemDetails();

builder.Host.UseLogger();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseRequestLogging();

app.UseMiddleware<ErrorMiddleware>();

app.UseAuthorization();

app.MapControllers();

await app.Services.RunStartupTasksAsync();

await app.RunAsync();

public partial class Program { }