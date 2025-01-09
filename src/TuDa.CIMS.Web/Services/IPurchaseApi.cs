﻿using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Extensions;
using TuDa.CIMS.Shared.Dtos;


namespace TuDa.CIMS.Web.Services;

/// <summary>
/// Refit client interface for the purchase API.
/// </summary>
[RefitClient("/api/purchases")]
public interface IPurchaseApi
{
    /// <summary>
    /// Retrives a purchase with the specified id.
    /// </summary>
    /// <param name="id"> the unique identifier of the purchase</param>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <returns>
    /// A task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing the purchase if successful
    /// </returns>
    public async Task<ErrorOr<Purchase>> GetAsync(Guid workingGroupId, Guid id) =>
        await GetAsyncInternal(workingGroupId, id).ToErrorOrAsync();

    /// <summary>
    /// Internal method to retrieve a purchase by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the purchase.</param>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse{T}"/> containing the Purchase.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Get("/{workingGroupId}/{id}")]
    protected Task<IApiResponse<Purchase>> GetAsyncInternal(Guid workingGroupId, Guid id);

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
    [Get("/{workingGroupId}")]
    protected Task<IApiResponse<IEnumerable<Purchase>>> GetAllAsyncInternal(Guid workingGroupId);

    /// <summary>
    /// Creates a new purchase.
    /// </summary>
    /// <param name="workingGroupId">Unique Id of the workinggroup</param>
    /// <param name="createPurchaseDto">The purchase to create.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing the unique identifier of the created purchase if successful.
    /// </returns>
    public async Task<ErrorOr<Guid>> CreateAsync(Guid workingGroupId, CreatePurchaseDto createPurchaseDto) =>
        await CreateAsyncInternal(workingGroupId, createPurchaseDto).ToErrorOrAsync();

    /// <summary>
    /// Internal method to create a new purchase.
    /// </summary>
    /// <param name="workingGroupId">Unique Id of the workinggroup</param>
    /// <param name="createPurchaseDto">The JSON serialized representation of the purchase.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse{T}"/> containing the unique identifier of the created purchase.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Post("/{workingGroupId}")]
    protected Task<IApiResponse<Guid>> CreateAsyncInternal(Guid workingGroupId, [Body] CreatePurchaseDto createPurchaseDto);

    /// <summary>
    /// Removes a purchase by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the purchase to remove.</param>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing a status indicating the deletion.
    /// </returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync( Guid workingGroupId, Guid id) =>
        await RemoveAsyncInternal(workingGroupId, id).ToErrorOrAsync<Deleted>();

    /// <summary>
    /// Internal method to remove a purchase by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the purchase to remove.</param>
    /// <param name="workingGroupId">the specific ID of a workinggroup</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse"/>.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Delete("/{workingGroupId}/{id}")]
    protected Task<IApiResponse> RemoveAsyncInternal(Guid workingGroupId, Guid id);
}
