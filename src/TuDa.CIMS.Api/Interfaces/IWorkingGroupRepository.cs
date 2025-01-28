﻿using System.Collections;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IWorkingGroupRepository
{
    Task<WorkingGroup?> GetOneAsync(Guid id);
    Task<List<WorkingGroup>> GetAllAsync();
    Task<ErrorOr<WorkingGroup>> UpdateAsync(Guid id, UpdateWorkingGroupDto updateModel);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id);

    Task<ErrorOr<WorkingGroup>> CreateAsync(CreateWorkingGroupDto createModel);
    Task<List<WorkingGroup>> SearchAsync(string name);
}
