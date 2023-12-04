using TMS.Common.Extensions;

using TMS.Ticketing.API;
using TMS.Ticketing.Persistence;
using TMS.Ticketing.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddApplicationServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);
    
var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseAuthorization();

app.UseMiddleware<ErrorMiddleware>();

app.MapControllers();

await app.Services.RunStartupTasksAsync();

await app.RunAsync();

public partial class Program { }