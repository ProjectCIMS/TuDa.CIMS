﻿using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Dtos.Update;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/working-groups/{workingGroupId:guid}/students")]
public class StudentController : CIMSBaseController
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>
    /// Updates an existing Student by its ID using the provided update model.
    /// If the update is successful, returns a 200 OK response.
    /// If an error occurs during the update, an appropriate error response is returned.
    /// </summary>
    /// <param name="workingGroupId">the unique id of the Working Group</param>
    /// <param name="id">the unique id of the Student</param>
    /// <param name="updateModel">the model containing the updated values for the Student </param>
    [HttpPatch($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> UpdateAsync(
        Guid workingGroupId,
        Guid id,
        UpdateStudentDto updateModel
    )
    {
        return (await _studentService.UpdateAsync(workingGroupId, id, updateModel)).Match(
            onValue: _ => Ok(),
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Deletes an existing Student from a Working Group by its ID.
    /// If the removal is successful, returns a 200 OK response and the Working Group.
    /// If an error occurs during the deletion, an appropriate error response is returned.
    ///</summary>
    /// <param name="id">the unique id of the Student</param>
    /// <param name="workingGroupId">the unique id of the Working Group</param>
    [HttpDelete($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> RemoveAsync(Guid workingGroupId, Guid id)
    {
        return (await _studentService.RemoveAsync(workingGroupId, id)).Match(
            onValue: _ => Ok(),
            onError: ErrorsToProblem
        );
    }

    ///  <summary>
    ///  Adds an existing Student to a Working Group by its ID.
    ///  If it is successful, returns a 200 OK response  and the Working Group.
    ///  If an error occurs during the process, an appropriate error response is returned.
    ///  </summary>
    ///  <param name="workingGroupId">the unique id of the Working Group</param>
    ///  <param name="createStudentDto">model to create a student</param>
    [HttpPost]
    public async Task<IActionResult> AddAsync(
        Guid workingGroupId,
        CreateStudentDto createStudentDto
    )
    {
        return (await _studentService.AddAsync(workingGroupId, createStudentDto)).Match(
            onValue: Ok,
            onError: ErrorsToProblem
        );
    }
}
