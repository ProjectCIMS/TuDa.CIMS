using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Dtos.Update;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IWorkingGroupService
{
    Task<ErrorOr<WorkingGroup>> GetOneAsync(Guid id);
    Task<ErrorOr<List<WorkingGroup>>> GetAllAsync(string? name);
    Task<ErrorOr<WorkingGroup>> UpdateAsync(Guid id, UpdateWorkingGroupDto updateModel);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id);

    Task<ErrorOr<WorkingGroup>> CreateAsync(CreateWorkingGroupDto createModel);

    Task<ErrorOr<Success>> ToggleActiveAsync(Guid id);
}
