using TMS.Common.Extensions;
using TMS.Common.Users;
using TMS.Payments.Application.UseCases;
using TMS.Payments.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddPeristence(builder.Configuration)
    .AddScoped<IUserContext, UserContext>()
    .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<CreatePaymentCommand>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<ErrorMiddleware>();

app.UseAuthorization();

app.MapControllers();

await app.Services.RunStartupTasksAsync();

await app.RunAsync();