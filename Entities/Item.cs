using System;

namespace Catalog.Entities
{
  public record Item
  {
    /*
      Recoard
      - Use for immutable objects
      - With-expression support
      - Value-based equality support

      init
      - only allow getting the values when init
      - Can do this
        Item item = new() { Id = Guid.NewGuid() }
        // Can't do this: item.Id = Guid.NewGuid();
    */
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
  }
}
