namespace TMS.Ticketing.Application.Services;

public interface IPaymentsService
{
    Task CreatePaymentAsync(Guid id, decimal amount);
}

public class PaymentsService : IPaymentsService
{
    public Task CreatePaymentAsync(Guid id, decimal amount)
    {
        return Task.CompletedTask;
    }
}
