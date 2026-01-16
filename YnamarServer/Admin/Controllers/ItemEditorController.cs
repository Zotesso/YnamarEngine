using Microsoft.AspNetCore.Mvc;
using YnamarServer.Database.Models;
using YnamarServer.Database.Models.Items;
using YnamarServer.Database.Protos;

namespace YnamarServer.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemEditorController : ControllerBase
    {
        [HttpGet("item/list")]
        [Produces("application/x-protobuf")]
        public async Task<IActionResult> GetItemList()
        {
            ItemList? itemList = await Program.itemEditorService.GetAllItemsSummaryAsync();

            if (itemList == null) return NotFound();

            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, itemList);
            return File(ms.ToArray(), "application/x-protobuf");
        }

        [HttpGet("item/type/list")]
        [Produces("application/x-protobuf")]
        public async Task<IActionResult> GetItemTypeList()
        {
            List<ItemType> itemTypeList = await Program.itemEditorService.GetNpcItemTypeListAsync();

            if (itemTypeList == null) return NotFound();

            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, itemTypeList);
            return File(ms.ToArray(), "application/x-protobuf");
        }

        [HttpGet("item/summary/{id}")]
        [Produces("application/x-protobuf")]
        public async Task<IActionResult> GetItemSummary(int id)
        {
            Item? requestedItemSummary = await Program.itemEditorService.GetItemSummary(id);

            if (requestedItemSummary == null) return NotFound();

            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, requestedItemSummary);
            return File(ms.ToArray(), "application/x-protobuf");
        }

        [HttpPost("item/save")]
        [Consumes("application/x-protobuf")]
        public async Task<IActionResult> SaveItem([FromBody] Item item)
        {
            int affectedRows = await Program.itemEditorService.SaveItemAsync(item);

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
