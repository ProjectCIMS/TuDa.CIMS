using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Extensions;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/working-groups")]
public class WorkingGroupController : CIMSBaseController
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
    [ProducesResponseType<List<WorkingGroupResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync([FromQuery] string? name)
    {
        return (await _workingGroupService.GetAllAsync(name)).Match(
            onValue: wgs => Ok(wgs.ToResponseDtos()),
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Retrieves a Working Group with the specific id from the service and returns it in an appropriate HTTP response.
    /// </summary>
    /// <param name="id">the unique id of the Working Group</param>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet($"{{{nameof(id)}:guid}}")]
    [ProducesResponseType<WorkingGroupResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOneAsync(Guid id)
    {
        return (await _workingGroupService.GetOneAsync(id)).Match(
            onValue: wg => Ok(wg.ToResponseDto()),
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Updates an existing Working Group by its ID using the provided update model.
    /// If the update is successful, returns a 200 OK response and the Working Group.
    /// If an error occurs during the update, an appropriate error response is returned.
    /// </summary>
    /// <param name="id">the unique id of the Working Group</param>
    /// <param name="updateModel">the model containing the updated values for the Working Group </param>
    [HttpPatch($"{{{nameof(id)}:guid}}")]
    [ProducesResponseType<WorkingGroupResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateWorkingGroupDto updateModel)
    {
        return (await _workingGroupService.UpdateAsync(id, updateModel)).Match(
            onValue: wg => Ok(wg.ToResponseDto()),
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Deletes an existing Working Group by its ID.
    /// If the removal is successful, returns a 200 OK response.
    /// If an error occurs during the deletion, an appropriate error response is returned.
    ///</summary>
    /// <param name="id">the unique id of the Working Group</param>
    [HttpDelete($"{{{nameof(id)}:guid}}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAsync(Guid id)
    {
        return (await _workingGroupService.RemoveAsync(id)).Match(
            onValue: _ => Ok(),
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Creates a new Working Group using the provided create model.
    /// If the removal is successful, returns a 200 OK response and the Working Group.
    /// If an error occurs during the deletion, an appropriate error response is returned.
    /// </summary>
    /// <param name="createModel">the model containing the data for the new Working Group</param>
    [HttpPost]
    [ProducesResponseType<WorkingGroupResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAsync(CreateWorkingGroupDto createModel)
    {
        return (await _workingGroupService.CreateAsync(createModel)).Match(
            onValue: wg => Ok(wg.ToResponseDto()),
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Deactivates or reactivates a Working Group by its ID.
    /// If the operation is successful, returns a 200 OK response.
    /// If an error occurs during the operation, an appropriate error response is returned.
    /// </summary>
    /// <param name="id">The id of the deactivated/reactivated working group</param>
    [HttpPatch($"{{{nameof(id)}:guid}}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleActiveAsync(Guid id)
    {
        return (await _workingGroupService.ToggleActiveAsync(id)).Match(
            onValue: _ => Ok(),
            onError: ErrorsToProblem
        );
    }
}
