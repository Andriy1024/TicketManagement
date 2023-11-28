using TMS.Common.Extensions;
using TMS.Common.Users;

using TMS.Payments.API;
using TMS.Payments.Application.MessageBrocker;
using TMS.Payments.Application.Ticketing;
using TMS.Payments.Application.UseCases;
using TMS.Payments.Persistence;

using Refit;

var builder = WebApplication.CreateBuilder(args);

var ticketingConfig = builder.Configuration.GetSection(nameof(TicketingConfig)).Get<TicketingConfig>()
    ?? throw new ArgumentNullException(nameof(TicketingConfig));

builder.Services
    .AddControllers()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddPeristence(builder.Configuration)
    .AddScoped<IUserContext, UserContext>()
    .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<CreatePaymentCommand>())
    .AddTransient<IMessageBrocker, MessageBrocker>()
    .AddRefitClient<ITicketingApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(ticketingConfig.TicketingUri));

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseMiddleware<ErrorMiddleware>();

app.UseAuthorization();

app.MapControllers();

await app.Services.RunStartupTasksAsync();

await app.RunAsync();