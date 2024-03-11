using Microsoft.AspNetCore.Mvc;
using Vyapari.Data;
using Vyapari.Service;

namespace Vyapari.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlackListRouteController : ControllerBase
    {
        private readonly IBlackListRouteService _service;

        public BlackListRouteController(IBlackListRouteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddBlackListRoute(BlackListRoute route)
        {
            var result = await _service.AddBlackListRoute(route);
            return CreatedAtAction(nameof(AddBlackListRoute), new { id = result.Id }, result);
        }

        // Implement other necessary methods here
    }
}