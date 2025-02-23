using System.ComponentModel.DataAnnotations.Schema;

namespace TuDa.CIMS.Shared.Entities;

/// <summary>
/// An entity representing a working group.
/// </summary>
public record WorkingGroup : BaseEntity
{
    /// <summary>
    /// The professor of the group.
    /// </summary>
    [ForeignKey("ProfessorId")]
    public required Professor Professor { get; set; }

    /// <summary>
    /// All students of the group.
    /// </summary>
    public List<Student> Students { get; set; } = [];

    /// <summary>
    /// A phone number to contact the group.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// An email to contact the group.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// All purchases done by the group.
    /// </summary>
    public List<Purchase> Purchases { get; set; } = [];

    /// <summary>
    /// Every group can be deactivated or activated.
    /// </summary>
    public bool IsDeactivated { get; set; } = false;
}
