using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Extensions;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/working-groups/{workingGroupId:guid}/purchases")]
public class PurchaseController : CIMSBaseController
{
    private readonly IPurchaseService _purchaseService;

    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    /// <summary>
    /// Retrieves all purchases from the service and returns them in an appropriate HTTP response.
    /// </summary>
    /// <param name="workingGroupId">the unique id of a workinggroup </param>
    /// <returns>a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet]
    [ProducesResponseType<List<PurchaseResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAsync(Guid workingGroupId)
    {
        return (await _purchaseService.GetAllAsync(workingGroupId)).Match(
            onValue: p => Ok(p.ToResponseDtos()),
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Retrieves a purchase with the specific id from the service and returns it in an appropriate HTTP response.
    /// </summary>
    /// <param name="purchaseId">the unique id of the purchase</param>
    /// <param name="workingGroupId">the unique id of a workinggroup </param>
    /// <returns>a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs  </returns>
    [HttpGet($"{{{nameof(purchaseId)}:guid}}")]
    [ProducesResponseType<PurchaseResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOneAsync(Guid workingGroupId, Guid purchaseId)
    {
        return (await _purchaseService.GetOneAsync(workingGroupId, purchaseId)).Match(
            onValue: wg => Ok(wg.ToResponseDto()),
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Removes a purchase with the specific id from the service.
    /// If the removal is successful, returns a 200 OK response. If an error occurs during the removal, an appropriate error response is returned.
    /// </summary>
    /// <param name="purchaseId">the unique id of the purchase</param>
    /// <param name="workingGroupId">the unique id of a workinggroup </param>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpDelete($"{{{nameof(purchaseId)}:guid}}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAsync(Guid workingGroupId, Guid purchaseId)
    {
        return (await _purchaseService.RemoveAsync(workingGroupId, purchaseId)).Match(
            onValue: _ => Ok(),
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Creates a new purchase using the provided create model.
    /// If the creation is successful, returns a 200 OK response. If an error occurs during the creation, an appropriate error response is returned.
    /// </summary>
    /// <param name="createModel">the model containing the data for the new purchase</param>
    /// <param name="workingGroupId">the unique id of a workinggroup </param>
    /// <returns>a 200 OK response and the object if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpPost]
    [ProducesResponseType<PurchaseResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateAsync(
        Guid workingGroupId,
        [FromBody] CreatePurchaseDto createModel
    )
    {
        return (await _purchaseService.CreateAsync(workingGroupId, createModel)).Match(
            onValue: p => Ok(p.ToResponseDto()),
            onError: ErrorsToProblem
        );
    }

    [HttpPatch($"{{{nameof(purchaseId)}:guid}}/invalidate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> InvalidateAsync(
        Guid workingGroupId,
        Guid purchaseId,
        [FromBody] CreatePurchaseDto createModel
    )
    {
        return (
            await _purchaseService.InvalidateAsync(workingGroupId, purchaseId, createModel)
        ).Match(onValue: _ => Ok(), onError: ErrorsToProblem);
    }

    [HttpGet($"{{{nameof(purchaseId)}:guid}}/signature")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RetrieveSignatureAsync(Guid workingGroupId, Guid purchaseId)
    {
        return (
            await _purchaseService.RetrieveSignatureAsync(workingGroupId, purchaseId)
        ).Match(onValue: value => Ok(value), onError: ErrorsToProblem);
    }
}
