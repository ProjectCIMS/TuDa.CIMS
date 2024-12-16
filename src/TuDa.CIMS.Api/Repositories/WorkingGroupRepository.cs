using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

public class WorkingGroupRepository : IWorkingGroupRepository
{
    private readonly CIMSDbContext _context;

    public WorkingGroupRepository(CIMSDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns an existing Working Group with the specific id.
    /// </summary>
    /// <param name="id">the specific id of the Working Group</param>
    /// <returns></returns>
    public async Task<WorkingGroup?> GetOneAsync(Guid id)
    {
        return await _context
            .WorkingGroups.Where(i => i.Id == id)
            .Include(workingGroup => workingGroup.Professor)
            .Include(workingGroup => workingGroup.Students)
            .Include(workingGroup => workingGroup.Purchases)
            .SingleOrDefaultAsync();
    }

    /// <summary>
    /// Returns all existing Working Groups of the database.
    /// </summary>
    public async Task<IEnumerable<WorkingGroup>> GetAllAsync()
    {
        return await _context
            .WorkingGroups.Include(workingGroup => workingGroup.Professor)
            .Include(workingGroup => workingGroup.Students)
            .Include(workingGroup => workingGroup.Purchases)
            .ToListAsync();
    }

    /// <summary>
    /// Updates an existing Working Group with the specified id using the provided update model.
    /// </summary>
    /// <param name="id">the specific id of the Working Group</param>
    /// <param name="updateModel">the model containing the updated values for the Working Group </param>
    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateWorkingGroupDto updateModel)
    {
        var existingItem = await _context
            .WorkingGroups.Where(i => i.Id == id)
            .Include(workingGroup => workingGroup.Professor)
            .Include(workingGroup => workingGroup.Students)
            .Include(workingGroup => workingGroup.Purchases)
            .SingleOrDefaultAsync();

        if (existingItem is null)
        {
            return Error.NotFound(
                "WorkingGroups.update",
                $"Working Group with id {id} was not found."
            );
        }

        existingItem.Professor = updateModel.Professor ?? existingItem.Professor;
        existingItem.Students = updateModel.Students ?? existingItem.Students;
        existingItem.PhoneNumber = updateModel.PhoneNumber ?? existingItem.PhoneNumber;
        existingItem.Purchases = updateModel.Purchases ?? existingItem.Purchases;

        await _context.SaveChangesAsync();
        return Result.Updated;
    }

    /// <summary>
    /// Removes a Working Group with the specific id from the database.
    /// </summary>
    /// <param name="id">the specific id of the Working Group</param>
    /// <returns></returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id)
    {
        var itemToRemove = await _context.WorkingGroups.SingleOrDefaultAsync(i => i.Id == id);

        if (itemToRemove is null)
        {
            return Error.NotFound(
                "WorkingGroups.remove",
                $"The working group with the id {id} was not found."
            );
        }

        _context.WorkingGroups.Remove(itemToRemove);

        await _context.SaveChangesAsync();
        return Result.Deleted;
    }

    /// <summary>
    /// Creates a Working Group and adds it to the database.
    /// </summary>
    /// <param name="createModel">the model containing the updated values for the Working Group</param>
    /// <returns></returns>
    public async Task<ErrorOr<Created>> CreateAsync(CreateWorkingGroupDto createModel)
    {
        var professor = await _context.Professors.FindAsync(createModel.Professor.Id);
        if (professor == null)
        {
            // Error if the professor for the new Working Group doesn't exist in the Professor table
            return Error.NotFound(
                "WorkingGroups.create",
                $"The professor with the id {createModel.Professor.Id} was not found."
            );
        }

        var studentIds = createModel.Students.Select(s => s.Id).ToList();
        var students = await _context.Students.Where(s => studentIds.Contains(s.Id)).ToListAsync();

        if (students.Count != createModel.Students.Count)
        {
            var missingStudentIds = studentIds.Except(students.Select(s => s.Id)).ToList();
            // Error if a student exists in the createModel but not in the database students table
            return Error.NotFound(
                "WorkingGroups.create",
                $"One or more students were not found. Missing students with ids: {string.Join(", ", missingStudentIds)}"
            );
        }

        var workingGroup = new WorkingGroup
        {
            Professor = createModel.Professor,
            PhoneNumber = createModel.PhoneNumber,
            Students = createModel.Students,
        };

        _context.WorkingGroups.Add(workingGroup);
        await _context.SaveChangesAsync();
        return Result.Created;
    }
}
