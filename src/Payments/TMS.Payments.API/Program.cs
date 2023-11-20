using TMS.Common.Extensions;
using TMS.Common.Users;

using TMS.Payments.API;
using TMS.Payments.Application.UseCases;
using TMS.Payments.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddPeristence(builder.Configuration)
    .AddScoped<IUserContext, UserContext>()
    .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<CreatePaymentCommand>());

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseMiddleware<ErrorMiddleware>();

app.UseAuthorization();

app.MapControllers();

await app.Services.RunStartupTasksAsync();

await app.RunAsync();