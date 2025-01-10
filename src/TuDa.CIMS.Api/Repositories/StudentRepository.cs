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
    public async Task<ErrorOr<WorkingGroup>> RemoveAsync(Guid id, Guid workingGroupId)
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
    /// <param name="id">specific ID of Student</param>
    /// <param name="workingGroupId">specific ID of Working Group</param>
    /// <returns>returns the Working Group in which the Student was added</returns>
    public async Task<ErrorOr<WorkingGroup>> AddAsync(Guid id, Guid workingGroupId)
    {
        var existingWorkingGroup = await _context
            .WorkingGroups.Where(i => i.Id == workingGroupId)
            .Include(workingGroup => workingGroup.Students)
            .SingleOrDefaultAsync();

        if (existingWorkingGroup is null)
        {
            return Error.NotFound("Student.add", $"Working Group with id {id} was not found.");
        }

        var existingStudent = await _context.Students.FindAsync(id);

        if (existingStudent == null)
        {
            return Error.NotFound("Student.add", $"Student with id {id} was not found.");
        }

        existingWorkingGroup.Students.Add(existingStudent);
        await _context.SaveChangesAsync();
        return existingWorkingGroup;
    }

    /// <summary>
    /// Updates an existing Student using the provided Values in the Update Model
    /// </summary>
    /// <param name="id">specific ID of the Student</param>
    /// <param name="updatedStudentDto">the model containing the updated values for the Student</param>
    /// <returns>Returns an Update Result</returns>
    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateStudentDto updatedStudentDto)
    {
        var existingStudent = await _context.Students.FindAsync(id);

        if (existingStudent == null)
        {
            return Error.NotFound("Student.update", $"Student with id {id} was not found.");
        }

        existingStudent.Name = updatedStudentDto.Name;
        existingStudent.FirstName = updatedStudentDto.FirstName;
        await _context.SaveChangesAsync();
        return Result.Updated;
    }
}
