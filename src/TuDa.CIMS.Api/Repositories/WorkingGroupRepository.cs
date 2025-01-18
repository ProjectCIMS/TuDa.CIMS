using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Api.Database;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Repositories;

[ScopedService]
public class WorkingGroupRepository : IWorkingGroupRepository
{
    private readonly CIMSDbContext _context;
    private readonly IProfessorRepository _professorRepository;

    public WorkingGroupRepository(CIMSDbContext context, IProfessorRepository professorRepository)
    {
        _context = context;
        _professorRepository = professorRepository;
    }

    private IQueryable<WorkingGroup> WorkingGroupsFilledQuery =>
        _context
            .WorkingGroups.Include(workingGroup => workingGroup.Professor)
            .ThenInclude(a => a.Address)
            .Include(workingGroup => workingGroup.Students)
            .Include(workingGroup => workingGroup.Purchases);

    private IQueryable<WorkingGroup> WorkingGroupsEmptyQuery => _context.WorkingGroups;

    /// <summary>
    /// Returns an existing Working Group with the specific id.
    /// </summary>
    /// <param name="id">the specific id of the Working Group</param>
    public Task<WorkingGroup?> GetOneAsync(Guid id) =>
        WorkingGroupsFilledQuery.SingleOrDefaultAsync(i => i.Id == id);

    /// <summary>
    /// Returns all existing Working Groups of the database.
    /// </summary>
    public Task<List<WorkingGroup>> GetAllAsync() => WorkingGroupsFilledQuery.ToListAsync();

    /// <summary>
    /// Updates an existing Working Group with the specified id using the provided update model.
    /// </summary>
    /// <param name="id">the specific id of the Working Group</param>
    /// <param name="updateModel">the model containing the updated values for the Working Group </param>
    /// <returns>Returns an Error or the updated Working Group</returns>
    public async Task<ErrorOr<WorkingGroup>> UpdateAsync(Guid id, UpdateWorkingGroupDto updateModel)
    {
        var existingItem = await GetOneAsync(id);

        if (existingItem is null)
        {
            return Error.NotFound(
                "WorkingGroups.update",
                $"Working Group with id {id} was not found."
            );
        }

        existingItem.Email = updateModel.Email ?? existingItem.Email;
        existingItem.PhoneNumber = updateModel.PhoneNumber ?? existingItem.PhoneNumber;

        if (updateModel.Professor is not null)
            await _professorRepository.UpdateAsync(
                existingItem.Professor.Id,
                updateModel.Professor
            );

        await _context.SaveChangesAsync();
        return existingItem;
    }

    /// <summary>
    /// Removes a Working Group with the specific id from the database.
    /// </summary>
    /// <param name="id">the specific id of the Working Group</param>
    /// <returns>Returns that the Working Group was successfully deleted</returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id)
    {
        var itemToRemove = await WorkingGroupsEmptyQuery.SingleOrDefaultAsync();

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
    /// <param name="createModel">The model containing the values for the newly created Working Group</param>
    /// <returns>Returns an Error or the created Working Group</returns>
    public async Task<ErrorOr<WorkingGroup>> CreateAsync(CreateWorkingGroupDto createModel)
    {
        // If the professor doesn't exist, create a new one
        var professor = await _professorRepository.CreateAsync(createModel.Professor);

        if (professor.IsError)
            return professor.Errors;

        // Create the WorkingGroup after ensuring all related entities exist
        var workingGroup = new WorkingGroup
        {
            Professor = professor.Value,
            PhoneNumber = createModel.PhoneNumber,
            Email = createModel.Email,
        };

        _context.WorkingGroups.Add(workingGroup);
        await _context.SaveChangesAsync();
        return workingGroup;
    }

    public async Task<List<WorkingGroup>> SearchAsync(string name)
    {
        return await WorkingGroupsFilledQuery
            .Where(s => EF.Functions.ILike(s.Professor.Name, $"%{name}%"))
            .ToListAsync();
    }
}
