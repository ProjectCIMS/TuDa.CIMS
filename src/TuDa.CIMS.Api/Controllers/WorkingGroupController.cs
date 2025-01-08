using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Dtos;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/working-groups")]
public class WorkingGroupController : ControllerBase
{
    private readonly IWorkingGroupService _workingGroupService;

    public WorkingGroupController(IWorkingGroupService workingGroupService)
    {
        _workingGroupService = workingGroupService;
    }

    /// <summary>
    /// Retrieves all Working Groups from the service and returns them in an appropriate HTTP response.
    /// </summary>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return (await _workingGroupService.GetAllAsync()).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }

    /// <summary>
    /// Retrieves a Working Group with the specific id from the service and returns it in an appropriate HTTP response.
    /// </summary>
    /// <param name="id">the unique id of the Working Group</param>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> GetOneAsync(Guid id)
    {
        return (await _workingGroupService.GetOneAsync(id)).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }

    /// <summary>
    /// Updates an existing Working Group by its ID using the provided update model.
    /// If the update is successful, returns a 200 OK response. If an error occurs during the update, an appropriate error response is returned.
    /// </summary>
    /// <param name="id">the unique id of the Working Group</param>
    /// <param name="updateModel">the model containing the updated values for the Working Group </param>
    [HttpPatch($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateWorkingGroupDto updateModel)
    {
        return (await _workingGroupService.UpdateAsync(id, updateModel)).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }

    /// <summary>
    /// Deletes an existing Working Group by its ID.
    /// If the removal is successful, returns a 200 OK response.
    /// If an error occurs during the deletion, an appropriate error response is returned.
    ///</summary>
    /// <param name="id">the unique id of the Working Group</param>
    [HttpDelete($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> RemoveAsync(Guid id)
    {
        return (await _workingGroupService.RemoveAsync(id)).Match<IActionResult>(
            _ => Ok(),
            err => BadRequest(err)
        );
    }

    /// <summary>
    /// Creates a new Working Group using the provided create model.
    /// If the removal is successful, returns a 201 Created response and the Working Group.
    /// If an error occurs during the deletion, an appropriate error response is returned.
    /// </summary>
    /// <param name="createModel">the model containing the data for the new Working Group</param>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateWorkingGroupDto createModel)
    {
        return (await _workingGroupService.CreateAsync(createModel)).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }
}
