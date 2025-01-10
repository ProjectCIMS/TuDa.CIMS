using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Services;

[ScopedService]
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="RemoveAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the Student</param>
    /// <param name="workingGroupId">the unique id of the Working Group</param>
    /// <returns></returns>
    public async Task<ErrorOr<WorkingGroup>> RemoveAsync(Guid id, Guid workingGroupId)
    {
        try
        {
            return await _studentRepository.RemoveAsync(id, workingGroupId);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "StudentService.RemoveAsync",
                $"Failed to remove Student with ID {id}. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="AddAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the Student</param>
    /// <param name="workingGroupId">the unique id of the Working Group</param>
    /// <returns></returns>
    public async Task<ErrorOr<WorkingGroup>> AddAsync(Guid id, Guid workingGroupId)
    {
        try
        {
            return await _studentRepository.AddAsync(id, workingGroupId);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "StudentService.AddAsync",
                $"Failed to add Student with ID {id}. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="UpdateAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the Student</param>
    /// <param name="updateModel">Model containing updated Values for the Student</param>
    /// <returns></returns>
    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateStudentDto updateModel)
    {
        try
        {
            return await _studentRepository.UpdateAsync(id, updateModel);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "Student.UpdateAsync",
                $"Failed to update Student with ID {id}. Exception: {e.Message}"
            );
        }
    }
}
