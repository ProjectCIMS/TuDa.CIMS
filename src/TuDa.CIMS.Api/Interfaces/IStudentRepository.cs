using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IStudentRepository
{
    Task<ErrorOr<WorkingGroup>> RemoveAsync(Guid id, Guid workingGroupId);
    Task<ErrorOr<WorkingGroup>> AddAsync(Guid id, Guid workingGroupId);
    Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateStudentDto updatedStudentDto);
}
