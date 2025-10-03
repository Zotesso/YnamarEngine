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
            int affectedRows = await Program.mapEditorService.SaveMapAsync(map);

            if (affectedRows > 0)
            {
                return Ok(affectedRows);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        [Consumes("application/x-protobuf")]
        [Produces("application/x-protobuf")]
        public async Task<IActionResult> GetMap(int id)
        {
            Map? requestedMap = await Program.mapEditorService.GetMapAsync(id);

            if (requestedMap == null) return NotFound();

            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, requestedMap);
            return File(ms.ToArray(), "application/x-protobuf");
        }
    }
}
