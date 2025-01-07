using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Services;

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
    public async Task<ErrorOr<WorkingGroup?>> GetOneAsync(Guid id)
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
    public async Task<ErrorOr<IEnumerable<WorkingGroup>>> GetAllAsync()
    {
        try
        {
            return (await _workingGroupRepository.GetAllAsync()).ToErrorOr();
        }
        catch (Exception e)
        {
            return Error.Failure(
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
                $"Failed to update Working Group with ID {id}. Exception: {e.Message + e.InnerException}"
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
    /// <returns></returns>
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
}
