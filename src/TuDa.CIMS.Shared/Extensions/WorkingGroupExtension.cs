using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Extensions;

public static class WorkingGroupExtension
{
    public static WorkingGroupResponseDto ToResponseDto(this WorkingGroup workingGroup) =>
        new()
        {
            Id = workingGroup.Id,
            Professor = workingGroup.Professor,
            Students = workingGroup.Students,
            PhoneNumber = workingGroup.PhoneNumber,
            Email = workingGroup.Email,
            Purchases = workingGroup.Purchases.Select(wg => wg.ToResponseDto()).ToList(),
        };

    public static List<WorkingGroupResponseDto> ToResponseDtos(
        this List<WorkingGroup> workingGroups
    ) => workingGroups.Select(ToResponseDto).ToList();
}
