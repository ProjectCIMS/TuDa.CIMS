using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IStudentService
{
    Task<ErrorOr<WorkingGroup>> RemoveAsync(Guid workingGroupId, Guid id);
    Task<ErrorOr<WorkingGroup>> AddAsync(
        Guid workingGroupId,
        Guid id,
        CreateStudentDto? createStudentDto
    );

    Task<ErrorOr<Updated>> UpdateAsync(
        Guid workingGroupId,
        Guid id,
        UpdateStudentDto updateStudentDto
    );
}
