using Microsoft.AspNetCore.Mvc;

using TMS.Notifications.Application.Interfaces;

namespace TMS.Notifications.API.Controllers;

[Route("api/notifications")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly INotificationsRepository _repository;

    public NotificationsController(INotificationsRepository repository)
        => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _repository.GetAllAsync();

        return Ok(result);
    }
}
