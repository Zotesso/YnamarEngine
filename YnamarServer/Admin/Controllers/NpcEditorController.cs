using Microsoft.AspNetCore.Mvc;
using YnamarServer.Database.Protos;

namespace YnamarServer.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NpcEditorController : ControllerBase
    {
        [HttpGet("listnpcs")]
        [Produces("application/x-protobuf")]
        public async Task<IActionResult> GetAllNpcsSummary()
        {
            NpcList? requestedNpcsSummary = await Program.npcEditorService.GetAllNpcsSummaryAsync();

            if (requestedNpcsSummary == null) return NotFound();

            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, requestedNpcsSummary);
            return File(ms.ToArray(), "application/x-protobuf");
        }
    }
}
