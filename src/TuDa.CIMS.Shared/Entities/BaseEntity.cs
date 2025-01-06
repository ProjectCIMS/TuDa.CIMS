using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuDa.CIMS.Shared.Entities;

public abstract record BaseEntity
{
    /// <summary>
    /// Unique id of the entity.
    /// </summary>
    /// <remarks>
    /// Guid must be empty to ensure the database is creating the ids itself.
    /// </remarks>
    [Key]
    public Guid Id { get; init; } = Guid.Empty;
}
