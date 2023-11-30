using TMS.Common.Extensions;

using TMS.Ticketing.API;
using TMS.Ticketing.Persistence;
using TMS.Ticketing.Application;

using TMS.Caching.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddApplicationServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration)
    .AddRedisServices(builder.Configuration);
    
var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseAuthorization();

app.UseMiddleware<ErrorMiddleware>();

app.MapControllers();

await app.Services.RunStartupTasksAsync();

await app.RunAsync();

public partial class Program { }

public class Foo 
{
    public string Name { get; set; }
}