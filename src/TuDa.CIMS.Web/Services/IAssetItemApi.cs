using System.Text.Json;
using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Services;

/// <summary>
/// Refit client for AssetItem operations.
/// </summary>
[RefitClient("/api/asset-items")]
public interface IAssetItemApi
{
    /// <summary>
    /// Gets an AssetItem by its ID.
    /// </summary>
    /// <param name="id">The ID of the AssetItem.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the AssetItem.</returns>
    [Get("/{id}")]
    public Task<AssetItem> GetAsync(Guid id);

    /// <summary>
    /// Gets all AssetItems.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, with a result of an IEnumerable of AssetItems.</returns>
    [Get("/")]
    public Task<IEnumerable<AssetItem>> GetAllAsync();

    /// <summary>
    /// Creates a new AssetItem.
    /// </summary>
    /// <param name="item">The AssetItem to create.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the created AssetItem's ID.</returns>
    public async Task<Guid> CreateAsync(AssetItem item) =>
        await CreateAsyncInternal(JsonSerializer.Serialize(item));

    /// <summary>
    /// Internal method to create a new AssetItem.
    /// </summary>
    /// <param name="item">The serialized AssetItem to create.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the created AssetItem's ID.</returns>
    [Put("/")]
    [Headers("Content-Type: application/json; charset=utf-8")]
    protected Task<Guid> CreateAsyncInternal([Body] string item);

    /// <summary>
    /// Removes an AssetItem by its ID.
    /// </summary>
    /// <param name="id">The ID of the AssetItem to remove.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    [Get("/{id}")]
    public Task RemoveAsync(Guid id);

    /// <summary>
    /// Updates an existing AssetItem.
    /// </summary>
    /// <param name="id">The ID of the AssetItem to update.</param>
    /// <param name="item">The updated AssetItem.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the updated AssetItem.</returns>
    public async Task<AssetItem> UpdateAsync(Guid id, AssetItem item) =>
        await UpdateAsyncInternal(id, JsonSerializer.Serialize(item));

    /// <summary>
    /// Internal method to update an existing AssetItem.
    /// </summary>
    /// <param name="id">The ID of the AssetItem to update.</param>
    /// <param name="item">The serialized updated AssetItem.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the updated AssetItem.</returns>
    [Patch("/{id}")]
    [Headers("Content-Type: application/json; charset=utf-8")]
    protected Task<AssetItem> UpdateAsyncInternal(Guid id, [Body] string item);
}
