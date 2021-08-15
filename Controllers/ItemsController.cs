using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using Catalog.Repo;
using System.Collections.Generic;
using Catalog.Entities;
using Catalog.Dtos;
using Gatelog;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
  // GET /items
  [ApiController]
  [Route("[controller]")] // follow default convention
  public class ItemsController : ControllerBase
  {
    private readonly IItemsRepository repo;

    public ItemsController(IItemsRepository repo)
    {
      this.repo = repo;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItemsAsync()
    {
      return (await repo.GetItemsAsync()).Select(item => item.AsDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
    {
      var item = await repo.GetItemAsync(id);
      if (item is null)
        return NotFound();

      return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
    {
      Item item = new Item()
      {
        Id = Guid.NewGuid(),
        Name = itemDto.Name,
        Price = itemDto.Price,
        CreatedDate = DateTimeOffset.UtcNow
      };

      await repo.CreateItemAsync(item);

      // route value will add a location to response header
      return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto itemDto)
    {
      var existingItem = repo.GetItemAsync(id);
      if (existingItem is null)
        return NotFound();

      // Feature of record type
      // Making a copy of existingItem with some changes
      Item upatedItem = await existingItem with
      {
        Name = itemDto.Name,
        Price = itemDto.Price
      };
      await repo.UpdateItemAsync(upatedItem);
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItem(Guid id)
    {
      var existingItem = repo.GetItemAsync(id);
      if (existingItem is null)
        return NotFound();

      await repo.DeleteItemAsync(id);
      return NoContent();
    }
  }
}
