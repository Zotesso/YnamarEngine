using Microsoft.AspNetCore.Mvc;
using YnamarServer.Database.Models;
using YnamarServer.Database.Protos;

namespace YnamarServer.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NpcEditorController : ControllerBase
    {
        [HttpGet("npcs/list")]
        [Produces("application/x-protobuf")]
        public async Task<IActionResult> GetNpcList()
        {
            NpcList? requestedNpcsSummary = await Program.npcEditorService.GetAllNpcsSummaryAsync();

            if (requestedNpcsSummary == null) return NotFound();

            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, requestedNpcsSummary);
            return File(ms.ToArray(), "application/x-protobuf");
        }

        [HttpGet("npcs/behavior/list")]
        [Produces("application/x-protobuf")]
        public async Task<IActionResult> GetNpcBehaviorList()
        {
            List<NpcBehavior> requestedNpcBehaviorList = await Program.npcEditorService.GetNpcBehaviorsListAsync();

            if (requestedNpcBehaviorList == null) return NotFound();

            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, requestedNpcBehaviorList);
            return File(ms.ToArray(), "application/x-protobuf");
        }

        [HttpGet("npcs/summary/{id}")]
        [Produces("application/x-protobuf")]
        public async Task<IActionResult> GetNpcSummary(int id)
        {
            Npc? requestedNpcSummary = await Program.npcEditorService.GetNpcSummary(id);

            if (requestedNpcSummary == null) return NotFound();

            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, requestedNpcSummary);
            return File(ms.ToArray(), "application/x-protobuf");
        }

        [HttpPost("npcs/save")]
        [Consumes("application/x-protobuf")]
        public async Task<IActionResult> SaveNpc([FromBody] Npc npc)
        {
            int affectedRows = await Program.npcEditorService.SaveNpcAsync(npc);

            if (affectedRows > 0)
            {
                return Ok(affectedRows);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
