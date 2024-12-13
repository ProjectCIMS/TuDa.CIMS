using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

public record UpdateWorkingGroupDto
{
    /// <summary>
    /// The professor of the group.
    /// </summary>
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
    /// All purchases done by the group.
    /// </summary>
    public List<Purchase> Purchases { get; set; } = [];
}
