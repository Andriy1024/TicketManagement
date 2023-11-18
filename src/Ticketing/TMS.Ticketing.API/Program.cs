using TMS.Common.Extensions;
using TMS.Common.Users;

using TMS.Ticketing.Persistence.Setup;
using TMS.Ticketing.API;
using TMS.Ticketing.Application.UseCases.Carts;
using TMS.Ticketing.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddScoped<IUserContext, UserContext>()
    .AddMongoServices(builder.Configuration)
    .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetCartDetails>())
    .AddTransient<IPaymentsService, PaymentsService>();

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