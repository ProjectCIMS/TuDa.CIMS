using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Dtos.Update;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IStudentService
{
    Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid id);
    Task<ErrorOr<Student>> AddAsync(Guid workingGroupId, CreateStudentDto createStudentDto);

    Task<ErrorOr<Updated>> UpdateAsync(
        Guid workingGroupId,
        Guid id,
        UpdateStudentDto updateStudentDto
    );
}
