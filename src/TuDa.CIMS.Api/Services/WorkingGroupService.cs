using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Services;

[ScopedService]
public class WorkingGroupService : IWorkingGroupService
{
    private readonly IWorkingGroupRepository _workingGroupRepository;

    public WorkingGroupService(IWorkingGroupRepository workingGroupRepository)
    {
        _workingGroupRepository = workingGroupRepository;
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetOneAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the Working Group</param>
    public async Task<ErrorOr<WorkingGroup>> GetOneAsync(Guid id)
    {
        try
        {
            return (await _workingGroupRepository.GetOneAsync(id)) switch
            {
                null => Error.NotFound(
                    "WorkingGroups.GetOneAsync",
                    $"Working Group with id {id} not found."
                ),
                var value => value,
            };
        }
        catch (Exception e)
        {
            return Error.Unexpected(
                "WorkingGroups.GetOneAsync",
                $"An unexpected error occurred: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Return an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetAllAsync"/> functionality if successful
    /// </summary>
    public async Task<ErrorOr<List<WorkingGroup>>> GetAllAsync(string? name)
    {
        try
        {
            return name != null
                ? await _workingGroupRepository.SearchAsync(name)
                : await _workingGroupRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            return name != null
                ? Error.Failure(
                    "WorkingGroups.SearchAsync",
                    $"Failed to search Working Groups with name {name}. Exception: {e.Message}"
                )
                : Error.Failure(
                    "WorkingGroups.GetAllAsync",
                    $"Failed to get all Working Groups. Exception: {e.Message}"
                );
        }
    }

    /// <summary>
    /// Return an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="UpdateAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the Working Group</param>
    /// <param name="updateModel">the model containing the updated values for the Working Group </param>
    public async Task<ErrorOr<WorkingGroup>> UpdateAsync(Guid id, UpdateWorkingGroupDto updateModel)
    {
        try
        {
            return await _workingGroupRepository.UpdateAsync(id, updateModel);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "WorkingGroups.UpdateAsync",
                $"Failed to update Working Group with ID {id}. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="RemoveAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the Working Group</param>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id)
    {
        try
        {
            return await _workingGroupRepository.RemoveAsync(id);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "Working Group.RemoveAsync",
                $"Failed to remove Working Group with ID {id}. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="CreateAsync"/> functionality if successful
    /// </summary>
    /// <param name="createModel">the model containing the values for the newly created Working Group</param>
    public async Task<ErrorOr<WorkingGroup>> CreateAsync(CreateWorkingGroupDto createModel)
    {
        try
        {
            return await _workingGroupRepository.CreateAsync(createModel);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "WorkingGroups.CreateAsync",
                $"Failed to create Working Group. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="ToggleActiveAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">The id of the deactivated/reactivated working group.</param>
    public async Task<ErrorOr<Success>> ToggleActiveAsync(Guid id)
    {
        try
        {
            return await _workingGroupRepository.ToggleActiveAsync(id);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "WorkingGroups.ToggleActiveAsync", e.Message);
        }
    }
}
