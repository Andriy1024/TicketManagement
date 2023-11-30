using Microsoft.AspNetCore.Mvc;

using TMS.Common.Caching;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TMS.Ticketing.API.Controllers;
[Route("api/redis")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ICoreCacheClient client;

    public TestController(ICoreCacheClient client)
    {
        this.client = client;
    }

    [HttpGet]
    public async Task<Foo?> Get([FromQuery] string key)
    {
        return await client.GetAsync<Foo>(key);
    }

    [HttpPost]
    public async Task Post(
        [FromQuery] string key,
        [FromBody] Foo foo)
    {
        await client.AddAsync(key, foo, TimeSpan.FromMinutes(2));
    }
}
