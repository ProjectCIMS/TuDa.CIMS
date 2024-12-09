using System.ComponentModel.DataAnnotations;

namespace TuDa.CIMS.Shared.Entities;

public abstract record BaseEntity
{
    /// <summary>
    /// Unique id of the entity.
    /// </summary>
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
}
