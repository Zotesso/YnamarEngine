using Microsoft.AspNetCore.Mvc;
using YnamarServer.Database.Models;

namespace YnamarServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapEditorController : ControllerBase
    {
        [HttpPost]
        [Consumes("application/x-protobuf")]
        public IActionResult SaveMap([FromBody] Map map)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetMap()
        {
            return Ok();
        }
    }
}
