using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IWorkingGroupService
{
    Task<ErrorOr<WorkingGroup?>> GetOneAsync(Guid id);
    Task<ErrorOr<IEnumerable<WorkingGroup>>> GetAllAsync();
    Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateWorkingGroupDto updateModel);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id);

    Task<ErrorOr<WorkingGroup>> CreateAsync(CreateWorkingGroupDto createModel);

    Task<ErrorOr<Updated>> AddStudentsAsync(Guid id, UpdateWorkingGroupDto updateModel);

    Task<ErrorOr<Updated>> DeleteStudentsAsync(Guid id, UpdateWorkingGroupDto updateModel);
}
