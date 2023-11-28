using TMS.Common.Extensions;
using TMS.Common.Users;

using TMS.Ticketing.Persistence.Setup;
using TMS.Ticketing.API;
using TMS.Ticketing.Application.UseCases.Carts;
using TMS.Ticketing.Application.Services.Payments;

using Refit;

var builder = WebApplication.CreateBuilder(args);

var paymentsConfig = builder.Configuration.GetSection(nameof(PaymentsConfig)).Get<PaymentsConfig>()
    ?? throw new ArgumentNullException(nameof(PaymentsConfig));

builder.Services
    .AddControllers()
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddScoped<IUserContext, UserContext>()
    .AddMongoServices(builder.Configuration)
    .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetCartDetails>())
    .AddTransient<IPaymentsService, PaymentsService>()
    .AddRefitClient<IPaymentsApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(paymentsConfig.PaymentsUri));

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseAuthorization();

app.UseMiddleware<ErrorMiddleware>();

app.MapControllers();

await app.Services.RunStartupTasksAsync();

await app.RunAsync();