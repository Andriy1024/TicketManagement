namespace TMS.Ticketing.Application.Interfaces;

public interface IPaymentsService
{
    Task CreatePaymentAsync(Guid id, decimal amount, int accointId);
}