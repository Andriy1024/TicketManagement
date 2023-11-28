namespace TMS.Ticketing.Application.Services.Payments;

public interface IPaymentsService
{
    Task CreatePaymentAsync(Guid id, decimal amount, int accointId);
}