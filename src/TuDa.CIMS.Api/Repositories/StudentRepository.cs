using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class StudentRepository : IStudentRepository
{
    private readonly CIMSDbContext _context;

    public StudentRepository(CIMSDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Removes a Student from a Working Group
    /// </summary>
    /// <param name="id">specific ID of Student</param>
    /// <param name="workingGroupId">specific ID of Working Group</param>
    /// <returns>returns the Working Group in which the Student was removed</returns>
    public async Task<ErrorOr<WorkingGroup>> RemoveAsync(Guid workingGroupId, Guid id)
    {
        var existingWorkingGroup = await _context
            .WorkingGroups.Where(i => i.Id == workingGroupId)
            .Include(workingGroup => workingGroup.Students)
            .SingleOrDefaultAsync();

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
        return existingWorkingGroup;
    }

    /// <summary>
    /// Adds a Student from a Working Group
    /// </summary>
    /// <param name="workingGroupId">specific ID of Working Group</param>
    /// <param name="createStudentDto">model to create a student if necessary</param>
    /// <returns>returns the Working Group in which the Student was added</returns>
    public async Task<ErrorOr<WorkingGroup>> AddAsync(
        Guid workingGroupId,
        CreateStudentDto createStudentDto
    )
    {
        var existingWorkingGroup = await _context
            .WorkingGroups.Where(i => i.Id == workingGroupId)
            .Include(workingGroup => workingGroup.Students)
            .SingleOrDefaultAsync();

        if (existingWorkingGroup is null)
        {
            return Error.NotFound(
                "Student.add",
                $"Working Group with id {workingGroupId} was not found."
            );
        }

        Student newStudent = new Student()
        {
            Name = createStudentDto?.Name ?? string.Empty,
            FirstName = createStudentDto?.FirstName ?? string.Empty,
        };
        _context.Students.Add(newStudent);

        existingWorkingGroup.Students.Add(newStudent);
        _context.Students.Add(newStudent);
        await _context.SaveChangesAsync();
        return existingWorkingGroup;
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
        var existingStudent = await _context.Students.FindAsync(id);

        if (existingStudent == null)
        {
            return Error.NotFound("Student.update", $"Student with id {id} was not found.");
        }

        var existingWorkingGroup = await _context
            .WorkingGroups.Where(i => i.Id == workingGroupId)
            .Include(workingGroup => workingGroup.Students)
            .SingleOrDefaultAsync();

        if (existingWorkingGroup == null)
        {
            return Error.NotFound("Student.update", $"WorkingGroup with id {id} was not found.");
        }

        foreach (Student student in existingWorkingGroup.Students)
        {
            if (existingStudent == student)
            {
                student.Name = updatedStudentDto.Name;
                student.FirstName = updatedStudentDto.FirstName;
            }
        }

        existingStudent.Name = updatedStudentDto.Name;
        existingStudent.FirstName = updatedStudentDto.FirstName;
        await _context.SaveChangesAsync();
        return Result.Updated;
    }
}
