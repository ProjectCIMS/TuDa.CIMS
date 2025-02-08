﻿using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Extensions;

namespace TuDa.CIMS.Web.Services;

/// <summary>
/// Refit client interface for the purchase API.
/// </summary>
[RefitClient("/api/working-groups")]
public interface IPurchaseApi
{
    /// <summary>
    /// Retries a purchase with the specified id.
    /// </summary>
    /// <param name="purchaseId"> the unique identifier of the purchase</param>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <returns>
    /// A task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing the purchase if successful
    /// </returns>
    public async Task<ErrorOr<Purchase>> GetAsync(Guid workingGroupId, Guid purchaseId) =>
        await GetAsyncInternal(workingGroupId, purchaseId).ToErrorOrAsync();

    /// <summary>
    /// Internal method to retrieve a purchase by its unique identifier.
    /// </summary>
    /// <param name="purchaseId">The unique identifier of the purchase.</param>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse{T}"/> containing the Purchase.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Get($"/{{{nameof(workingGroupId)}}}/purchases/{{{nameof(purchaseId)}}}")]
    protected Task<IApiResponse<Purchase>> GetAsyncInternal(Guid workingGroupId, Guid purchaseId);

    /// <summary>
    /// Retrieves all purchases.
    /// </summary>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing an IEnumerable of purchases if successful.
    /// </returns>
    public async Task<ErrorOr<IEnumerable<Purchase>>> GetAllAsync(Guid workingGroupId) =>
        await GetAllAsyncInternal(workingGroupId).ToErrorOrAsync();

    /// <summary>
    /// Internal method to retrieve all purchases.
    /// </summary>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse{T}"/> containing an IEnumerable of purchases.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Get($"/{{{nameof(workingGroupId)}}}/purchases/")]
    protected Task<IApiResponse<IEnumerable<Purchase>>> GetAllAsyncInternal(Guid workingGroupId);

    /// <summary>
    /// Creates a new purchase.
    /// </summary>
    /// <param name="workingGroupId">Unique Id of the workinggroup</param>
    /// <param name="createPurchaseDto">The purchase to create.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing the unique identifier of the created purchase if successful.
    /// </returns>
    public async Task<ErrorOr<Purchase>> CreateAsync(
        Guid workingGroupId,
        CreatePurchaseDto createPurchaseDto
    ) => await CreateAsyncInternal(workingGroupId, createPurchaseDto).ToErrorOrAsync();

    /// <summary>
    /// Internal method to create a new purchase.
    /// </summary>
    /// <param name="workingGroupId">Unique Id of the workinggroup</param>
    /// <param name="createPurchaseDto">The JSON serialized representation of the purchase.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse{T}"/> containing the unique identifier of the created purchase.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>

    [Post($"/{{{nameof(workingGroupId)}}}/purchases/")]
    protected Task<IApiResponse<Purchase>> CreateAsyncInternal(
        Guid workingGroupId,
        [Body] CreatePurchaseDto createPurchaseDto
    );

    /// <summary>
    /// Removes a purchase by its unique identifier.
    /// </summary>
    /// <param name="purchaseId">The unique identifier of the purchase to remove.</param>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing a status indicating the deletion.
    /// </returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid workingGroupId, Guid purchaseId) =>
        await RemoveAsyncInternal(workingGroupId, purchaseId).ToErrorOrAsync<Deleted>();

    /// <summary>
    /// Internal method to remove a purchase by its unique identifier.
    /// </summary>
    /// <param name="purchaseId">The unique identifier of the purchase to remove.</param>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse"/>.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Delete($"/{{{nameof(workingGroupId)}}}/purchases/{{{nameof(purchaseId)}}}")]
    protected Task<IApiResponse> RemoveAsyncInternal(Guid workingGroupId, Guid purchaseId);
}
