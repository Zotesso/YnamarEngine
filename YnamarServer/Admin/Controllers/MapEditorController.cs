using Microsoft.AspNetCore.Mvc;
using YnamarServer.Database.Models;

namespace YnamarServer.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapEditorController : ControllerBase
    {
        [HttpPost]
        [Consumes("application/x-protobuf")]
        public async Task<IActionResult> SaveMap([FromBody] Map map)
        {
            int affectedRows = await Program.mapEditorService.SaveMap(map);

            if (affectedRows > 0)
            {
                return Ok(affectedRows);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        public IActionResult GetMap()
        {
            return Ok();
        }
    }
}
