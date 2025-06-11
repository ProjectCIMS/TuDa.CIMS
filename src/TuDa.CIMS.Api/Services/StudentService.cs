using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Dtos.Update;
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
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid id)
    {
        try
        {
            return await _studentRepository.RemoveAsync(workingGroupId, id);
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
    /// <param name="workingGroupId">the unique id of the Working Group</param>
    /// <param name="createStudentDto">to add a student but optional</param>
    /// <returns></returns>
    public async Task<ErrorOr<Student>> AddAsync(
        Guid workingGroupId,
        CreateStudentDto createStudentDto
    )
    {
        try
        {
            return await _studentRepository.AddAsync(workingGroupId, createStudentDto);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "StudentService.AddAsync",
                $"Failed to add Student, Working Group not found with ID {workingGroupId}. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="UpdateAsync"/> functionality if successful
    /// </summary>
    /// <param name="workingGroupId">the unique id of the Working Group</param>
    /// <param name="id">the unique id of the Student</param>
    /// <param name="updateModel">Model containing updated Values for the Student</param>
    /// <returns></returns>
    public async Task<ErrorOr<Updated>> UpdateAsync(
        Guid workingGroupId,
        Guid id,
        UpdateStudentDto updateModel
    )
    {
        try
        {
            return await _studentRepository.UpdateAsync(workingGroupId, id, updateModel);
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
