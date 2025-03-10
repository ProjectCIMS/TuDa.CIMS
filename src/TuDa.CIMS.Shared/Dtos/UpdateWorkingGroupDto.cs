﻿using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos;

public record UpdateWorkingGroupDto
{
    /// <summary>
    /// The professor of the group.
    /// </summary>
    public UpdateProfessorDto? Professor { get; set; }

    /// <summary>
    /// A phone number to contact the group.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// An email to contact the group.
    /// </summary>
    public string? Email { get; set; }
}
