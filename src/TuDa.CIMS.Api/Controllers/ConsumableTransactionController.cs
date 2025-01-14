﻿using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Controllers;

[ApiController]
[Route("api/consumableTransaction")]
public class ConsumableTransactionController : CIMSBaseController
{
    private readonly IConsumableTransactionService _consumableTransactionService;

    public ConsumableTransactionController(IConsumableTransactionService consumableTransactionService)
    {
        _consumableTransactionService = consumableTransactionService;
    }

    /// <summary>
    /// Retrieves all ConsumableTransactions from the service and returns them in an appropriate HTTP response.
    /// </summary>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return (await _consumableTransactionService.GetAllAsync()).Match(
            onValue: Ok,
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Retrieves an ConsumableTransaction with the specific id from the service and returns it in an appropriate HTTP response.
    /// </summary>
    /// <param name="id">the unique id of the ConsumableTransaction</param>
    /// <returns> a 200 OK response if the operation is successfully and a 400 BadRequest response if any error occurs </returns>
    [HttpGet($"{{{nameof(id)}:guid}}")]
    public async Task<IActionResult> GetOneAsync(Guid id)
    {
        return (await _consumableTransactionService.GetOneAsync(id)).Match(
            onValue: value => Ok(JsonSerializer.Serialize(value)),
            onError: ErrorsToProblem
        );
    }

    /// <summary>
    /// Updates the amount of the specific Consumamble of the ConsumableTransaction and creates a new transaction for a consumable item.
    /// If this is successful, returns a 200 OK response. If an error occurs during the update, an appropriate error response is returned.
    /// </summary>
    /// <param name="createModel"></param>

   /* [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateConsumableTransactionDto createModel)
    {
        return (await _consumableTransactionService.CreateAsync(createModel)).Match(
            onValue: _ => Ok(),
            onError: ErrorsToProblem
        );
    }*/

    [HttpPost]
    public async Task<IActionResult> CreateForPurchaseAsync(Purchase purchase)
    {
        return (await _consumableTransactionService.CreateForPurchaseAsync(purchase)).Match(
            onValue: _ => Ok(),
            onError: ErrorsToProblem
        );
    }

}