﻿using System.Text.Json;
using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Web.Extensions;
using TuDa.CIMS.Web.Services;


namespace TuDa.CIMS.Web.Services;

/// <summary>
/// Refit client interface for performing operations on AssetItems.
/// </summary>
[RefitClient("/api/asset-items")]
public interface IAssetItemApi
{
    /// <summary>
    /// Retrieves an AssetItem by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the AssetItem.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing the AssetItem if successful.
    /// </returns>
    public async Task<ErrorOr<AssetItem>> GetAsync(Guid id) =>
        await GetAsyncInternal(id).ToErrorOrAsync();

    /// <summary>
    /// Internal method to retrieve an AssetItem by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the AssetItem.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse{T}"/> containing the AssetItem.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Get("/{id}")]
    protected Task<IApiResponse<AssetItem>> GetAsyncInternal(Guid id);

    /// <summary>
    /// Retrieves all AssetItems.
    /// </summary>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing an IEnumerable of AssetItems if successful.
    /// </returns>

    public async Task<ErrorOr<IEnumerable<AssetItem>>> GetAllAsync(string? nameOrCas = null) =>
        await GetAllAsyncInternal(nameOrCas).ToErrorOrAsync();


    /// <summary>
    /// Internal method to retrieve all AssetItems.
    /// </summary>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse{T}"/> containing an IEnumerable of AssetItems.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Get("/")]
    protected Task<IApiResponse<IEnumerable<AssetItem>>> GetAllAsyncInternal(
        [Query] string? nameOrCas
    );


    /// <summary>
    /// Creates a new AssetItem.
    /// </summary>
    /// <param name="item">The AssetItem to create.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing the unique identifier of the created AssetItem if successful.
    /// </returns>
    /// <remarks>
    /// Uses <see cref="JsonSerializer"/> to ensure proper serialization of polymorphic types.
    /// </remarks>
    [Obsolete("Not functioning at the moment. Only here for completeness. Need to be done by #41.")]
    public async Task<ErrorOr<Guid>> CreateAsync(AssetItem item) =>
        await CreateAsyncInternal(JsonSerializer.Serialize(item)).ToErrorOrAsync();

    /// <summary>
    /// Internal method to create a new AssetItem.
    /// </summary>
    /// <param name="item">The JSON serialized representation of the AssetItem.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse{T}"/> containing the unique identifier of the created AssetItem.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Put("/")]
    [Headers("Content-Type: application/json; charset=utf-8")]
    protected Task<IApiResponse<Guid>> CreateAsyncInternal([Body] string item);

    /// <summary>
    /// Removes an AssetItem by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the AssetItem to remove.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing a status indicating the deletion.
    /// </returns>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id) =>
        await RemoveAsyncInternal(id).ToErrorOrAsync<Deleted>();

    /// <summary>
    /// Internal method to remove an AssetItem by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the AssetItem to remove.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse"/>.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Delete("/{id}")]
    protected Task<IApiResponse> RemoveAsyncInternal(Guid id);

    /// <summary>
    /// Updates an existing AssetItem.
    /// </summary>
    /// <param name="id">The unique identifier of the AssetItem to update.</param>
    /// <param name="updateAssetItemDto">The updateAssetItemDto.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="ErrorOr{T}"/> containing a status indicating the update success.
    /// </returns>
    /// <remarks>
    /// Uses <see cref="JsonSerializer"/> to ensure proper serialization of polymorphic types.
    /// </remarks>
    public async Task<ErrorOr<Updated>> UpdateAsync(
        Guid id,
        UpdateAssetItemDto updateAssetItemDto
    ) =>
        await UpdateAsyncInternal(id, JsonSerializer.Serialize(updateAssetItemDto))
            .ToErrorOrAsync<Updated>();

    /// <summary>
    /// Internal method to update an existing AssetItem.
    /// </summary>
    /// <param name="id">The unique identifier of the AssetItem to update.</param>
    /// <param name="updateAssetItemDto">The JSON serialized representation of the updatedAssetItemDto.</param>
    /// <returns>
    /// A Task representing the asynchronous operation, with a result of <see cref="IApiResponse"/>.
    /// </returns>
    /// <remarks>This will be generated by <see cref="Refit"/>.</remarks>
    [Patch("/{id}")]
    [Headers("Content-Type: application/json; charset=utf-8")]
    protected Task<IApiResponse> UpdateAsyncInternal(Guid id, [Body] string updateAssetItemDto);
}
