﻿using System.Collections;
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

    public WorkingGroupRepository(CIMSDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns an existing Working Group with the specific id.
    /// </summary>
    /// <param name="id">the specific id of the Working Group</param>
    public async Task<WorkingGroup?> GetOneAsync(Guid id)
    {
        return await _context
            .WorkingGroups.Where(i => i.Id == id)
            .Include(workingGroup => workingGroup.Professor)
            .ThenInclude(a => a.Address)
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
            .ThenInclude(a => a.Address)
            .Include(workingGroup => workingGroup.Students)
            .Include(workingGroup => workingGroup.Purchases)
            .ToListAsync();
    }

    /// <summary>
    /// Updates an existing Working Group with the specified id using the provided update model.
    /// </summary>
    /// <param name="id">the specific id of the Working Group</param>
    /// <param name="updateModel">the model containing the updated values for the Working Group </param>
    /// <returns>Returns an Error or the updated Working Group</returns>
    public async Task<ErrorOr<WorkingGroup>> UpdateAsync(Guid id, UpdateWorkingGroupDto updateModel)
    {
        var existingItem = await _context
            .WorkingGroups.Where(i => i.Id == id)
            .Include(workingGroup => workingGroup.Professor)
            .ThenInclude(a => a.Address)
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

        existingItem.Email = updateModel.Email ?? existingItem.Email;
        existingItem.PhoneNumber = updateModel.PhoneNumber ?? existingItem.PhoneNumber;
        existingItem.Professor.Name = updateModel.Professor?.Name ?? existingItem.Professor.Name;
        existingItem.Professor.FirstName =
            updateModel.Professor?.FirstName ?? existingItem.Professor.FirstName;
        existingItem.Professor.Title = updateModel.Professor?.Title ?? existingItem.Professor.Title;
        existingItem.Professor.Gender = updateModel.Professor?.Gender ?? existingItem.Professor.Gender;
        existingItem.Professor.Address.City = updateModel.Professor?.Address.City ?? existingItem.Professor.Address.City;
        existingItem.Professor.Address.Street = updateModel.Professor?.Address.Street ?? existingItem.Professor.Address.Street;
        existingItem.Professor.Address.Number = updateModel.Professor?.Address.Number ?? existingItem.Professor.Address.Number;
        existingItem.Professor.Address.ZipCode = updateModel.Professor?.Address.ZipCode ?? existingItem.Professor.Address.ZipCode;

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
        var itemToRemove = await _context
            .WorkingGroups.Where(i => i.Id == id)
            .SingleOrDefaultAsync();

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
        var professor = await _context.Professors.FindAsync(createModel.Professor.Id);
        if (professor == null)
        {
            // If the professor doesn't exist, create a new one
            professor = new Professor
            {
                Name = createModel.Professor.Name, FirstName = createModel.Professor.FirstName,
            };
            _context.Professors.Add(professor);
        }

        // Create the WorkingGroup after ensuring all related entities exist
        var workingGroup = new WorkingGroup
        {
            Professor = createModel.Professor, PhoneNumber = createModel.PhoneNumber, Email = createModel.Email,
        };

        _context.WorkingGroups.Add(workingGroup);
        await _context.SaveChangesAsync();
        return workingGroup;
    }

    public async Task<IEnumerable<WorkingGroup>> SearchAsync(string name)
    {
        return await _context.WorkingGroups.Include(p => p.Professor)
            .Where(s => EF.Functions.ILike(s.Professor.Name, $"%{name}%")).ToListAsync();
    }
}
