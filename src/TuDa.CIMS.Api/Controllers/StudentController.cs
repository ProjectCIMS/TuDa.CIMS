using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Services;
using TuDa.CIMS.Shared.Dtos;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/working-groups/{workingGroupId}/students")]
public class StudentController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentController(StudentService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>
    /// Updates an existing Student by its ID using the provided update model.
    /// If the update is successful, returns a 200 OK response.
    /// If an error occurs during the update, an appropriate error response is returned.
    /// </summary>
    /// <param name="id">the unique id of the Student</param>
    /// <param name="updateModel">the model containing the updated values for the Student </param>
    [HttpPatch($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateStudentDto updateModel)
    {
        return (await _studentService.UpdateAsync(id, updateModel)).Match<IActionResult>(
            _ => Ok(),
            err => BadRequest(err)
        );
    }

    /// <summary>
    /// Deletes an existing Student from a Working Group by its ID.
    /// If the removal is successful, returns a 200 OK response and the Working Group.
    /// If an error occurs during the deletion, an appropriate error response is returned.
    ///</summary>
    /// <param name="id">the unique id of the Student</param>
    /// /// <param name="workingGroupId">the unique id of the Student</param>
    [HttpDelete($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> RemoveAsync(Guid id, Guid workingGroupId)
    {
        return (await _studentService.RemoveAsync(id, workingGroupId)).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }

    /// <summary>
    /// Adds an existing Student to a Working Group by its ID.
    /// If it is successful, returns a 200 OK response  and the Working Group.
    /// If an error occurs during the process, an appropriate error response is returned.
    ///</summary>
    /// <param name="id">the unique id of the Student</param>
    /// /// <param name="workingGroupId">the unique id of the Student</param>
    [HttpPost($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> AddAsync(Guid id, Guid workingGroupId)
    {
        return (await _studentService.AddAsync(id, workingGroupId)).Match<IActionResult>(
            value => Ok(value),
            err => BadRequest(err)
        );
    }
}
