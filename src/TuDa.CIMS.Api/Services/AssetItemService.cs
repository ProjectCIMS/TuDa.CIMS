using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Services;

public class AssetItemService : IAssetItemService
{
    private readonly IAssetItemRepository _assetItemRepository;

    public AssetItemService(IAssetItemRepository assetItemRepository)
    {
        _assetItemRepository = assetItemRepository;
    }

    /// <summary>
    /// Calls the <see cref="IAssetItemRepository.GetAllAsync"/> function of the repository and handles any error that occurs.
    /// </summary>
    /// <returns>  An <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetAllAsync"/> functionality if successful</returns>>
    public async Task<ErrorOr<IEnumerable<AssetItem>>> GetAllAsync()
    {
        try
        {
            var result = await _assetItemRepository.GetAllAsync();
            return result.ToErrorOr();
        }
        catch (Exception e)
        {
            return Error.Failure(
                "AssetItem.GetAllAsync",
                $"Failed to get all AssetItems. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Calls the <see cref="IAssetItemRepository.GetOneAsync"/> function of the repository and handles any error that occurs.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <returns>  An <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetOneAsync"/> functionality if successful</returns>>
    public async Task<ErrorOr<AssetItem>> GetOneAsync(Guid id)
    {
        try
        {
            return await _assetItemRepository.GetOneAsync(id);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "AssetItem.GetOneAsync",
                $"Failed to get AssetItem with ID {id}. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Calls the <see cref="IAssetItemRepository.UpdateAsync"/> function of the repository and handles any error that occurs.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <param name="updateModel">the model containing the updated values for the AssetItem </param>
    /// <returns>  An <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="UpdateAsync"/> functionality if successful</returns>>
    public async Task<ErrorOr<Success>> UpdateAsync(Guid id, AssetItem updateModel)
    {
        try
        {
            await _assetItemRepository.UpdateAsync(id, updateModel);
            return Result.Success;
        }
        catch (Exception e)
        {
            return Error.Failure(
                "AssetItem.UpdateAsync",
                $"Failed to update AssetItem with ID {id}. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Calls the <see cref="IAssetItemRepository.RemoveAsync"/> function of the repository and handles any error that occurs.
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <returns>  An <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="RemoveAsync"/> functionality if successful</returns>>
    public async Task<ErrorOr<Success>> RemoveAsync(Guid id)
    {
        try
        {
            await _assetItemRepository.RemoveAsync(id);
            return Result.Success;
        }
        catch (Exception e)
        {
            return Error.Failure(
                "AssetItem.RemoveAsync",
                $"Failed to remove AssetItem with ID {id}. Exception: {e.Message}"
            );
        }
    }
}
