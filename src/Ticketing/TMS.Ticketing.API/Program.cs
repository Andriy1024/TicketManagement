using TMS.Ticketing.Persistence.Setup;
using TMS.Common.Extensions;
using TMS.Ticketing.Domain.Venues;
using TMS.Ticketing.API;
using TMS.Common.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddScoped<IUserContext, UserContext>()
    .AddMongoServices(builder.Configuration)
    .AddMongoRepository<Venue, Guid>()
    .AddMongoRepository<VenueBooking, Guid>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseMiddleware<ErrorMiddleware>();

app.MapControllers();

await app.Services.RunStartupTasksAsync();

await app.RunAsync();