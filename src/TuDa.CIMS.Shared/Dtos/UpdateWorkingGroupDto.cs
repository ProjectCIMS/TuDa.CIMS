using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

public record UpdateWorkingGroupDto
{
    /// <summary>
    /// The professor of the group.
    /// </summary>
    public Professor? Professor { get; set; }

    /// <summary>
    /// All students of the group.
    /// </summary>
    public List<Student>? Students { get; set; } = [];

    /// <summary>
    /// A phone number to contact the group.
    /// </summary>
    public string? PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// All purchases done by the group.
    /// </summary>
    public List<Purchase>? Purchases { get; set; } = [];

    /// <summary>
    /// Students to be added to the group.
    /// </summary>
    public List<Student>? AddStudentsList { get; set; } = [];

    /// <summary>
    /// Students to be removed from the group.
    /// </summary>
    public List<Student>? RemoveStudentsList { get; set; } = [];
}
