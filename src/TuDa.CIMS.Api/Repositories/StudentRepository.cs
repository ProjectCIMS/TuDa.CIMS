using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class StudentRepository : IStudentRepository
{
    private readonly CIMSDbContext _context;
    private readonly IWorkingGroupRepository _workingGroupRepository;

    public StudentRepository(CIMSDbContext context, IWorkingGroupRepository workingGroupRepository)
    {
        _context = context;
        _workingGroupRepository = workingGroupRepository;
    }

    /// <summary>
    /// Removes a Student from a Working Group
    /// </summary>
    /// <param name="id">specific ID of Student</param>
    /// <param name="workingGroupId">specific ID of Working Group</param>
    /// <returns>returns the Working Group in which the Student was removed</returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid id)
    {
        var existingWorkingGroup = await _workingGroupRepository.GetOneAsync(workingGroupId);

        if (existingWorkingGroup is null)
        {
            return Error.NotFound("Student.remove", $"Working Group with id {id} was not found.");
        }

        var existingStudent = existingWorkingGroup.Students.SingleOrDefault(i => i.Id == id);

        if (existingStudent == null)
        {
            return Error.NotFound("Student.remove", $"Student with id {id} was not found.");
        }

        existingWorkingGroup.Students.Remove(existingStudent);
        await _context.SaveChangesAsync();

        return Result.Deleted;
    }

    /// <summary>
    /// Adds a Student from a Working Group
    /// </summary>
    /// <param name="workingGroupId">specific ID of Working Group</param>
    /// <param name="createStudentDto">model to create a student if necessary</param>
    /// <returns>returns the Working Group in which the Student was added</returns>
    public async Task<ErrorOr<Student>> AddAsync(
        Guid workingGroupId,
        CreateStudentDto createStudentDto
    )
    {
        var existingWorkingGroup = await _workingGroupRepository.GetOneAsync(workingGroupId);

        if (existingWorkingGroup is null)
        {
            return Error.NotFound(
                "Student.add",
                $"Working Group with id {workingGroupId} was not found."
            );
        }

        var newStudent = new Student()
        {
            Name = createStudentDto.Name,
            FirstName = createStudentDto.FirstName,
            Gender = createStudentDto.Gender,
            PhoneNumber = createStudentDto.PhoneNumber
        };

        existingWorkingGroup.Students.Add(newStudent);
        await _context.SaveChangesAsync();

        return newStudent;
    }

    /// <summary>
    /// Updates an existing Student using the provided Values in the Update Model
    /// </summary>
    /// <param name="workingGroupId">the unique id of the Working Group</param>
    /// <param name="id">the unique ID of the Student</param>
    /// <param name="updatedStudentDto">the model containing the updated values for the Student</param>
    /// <returns>Returns an Update Result</returns>
    public async Task<ErrorOr<Updated>> UpdateAsync(
        Guid workingGroupId,
        Guid id,
        UpdateStudentDto updatedStudentDto
    )
    {
        var existingWorkingGroup = await _workingGroupRepository.GetOneAsync(workingGroupId);

        if (existingWorkingGroup == null)
        {
            return Error.NotFound("Student.update", $"WorkingGroup with id {id} was not found.");
        }

        var existingStudent = existingWorkingGroup.Students.SingleOrDefault(s => s.Id == id);

        if (existingStudent == null)
        {
            return Error.NotFound("Student.update", $"Student with id {id} was not found.");
        }

        existingStudent.Name = updatedStudentDto.Name ?? existingStudent.Name;
        existingStudent.FirstName = updatedStudentDto.FirstName ?? existingStudent.FirstName;
        existingStudent.Gender = updatedStudentDto.Gender ?? existingStudent.Gender;
        existingStudent.PhoneNumber = updatedStudentDto.PhoneNumber ?? existingStudent.PhoneNumber;

        await _context.SaveChangesAsync();
        return Result.Updated;
    }
}
