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
