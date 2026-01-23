using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database.Models;
using YnamarServer.Database.Models.Animation;
using YnamarServer.Database.Protos;

namespace YnamarServer.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimationEditorController : ControllerBase
    {
        [HttpGet("animation/list")]
        [Produces("application/x-protobuf")]
        public async Task<IActionResult> GetAnimationClipList()
        {
            AnimationClipList? animationClipList = await Program.animationEditorService.GetAllAnimationClipsSummary();

            if (animationClipList == null) return NotFound();
            
            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, animationClipList);
            return File(ms.ToArray(), "application/x-protobuf");
        }

        [HttpGet("animation/{id}")]
        [Produces("application/x-protobuf")]
        public async Task<IActionResult> GetAnimationClip(int id)
        {
            AnimationClip? requestedAnimationClip = await Program.animationEditorService.GetAnimationClip(id);

            if (requestedAnimationClip == null) return NotFound();

            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, requestedAnimationClip);
            return File(ms.ToArray(), "application/x-protobuf");
        }

        [HttpPost("animation/save")]
        [Consumes("application/x-protobuf")]
        public async Task<IActionResult> SaveAnimationClip([FromBody] AnimationClip animation)
        {
            int affectedRows = await Program.animationEditorService.SaveAnimationClipAsync(animation);

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
