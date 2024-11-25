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

    public async Task<IEnumerable<AssetItem>> GetAllAsync()
    {
        try
        {
            var result = await _assetItemRepository.GetAllAsync();
            return result;
        }
        catch (Exception e)
        {
            return [];
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
            return Error.Failure("AssetItem.GetOne");
        }
    }

    public async Task UpdateAsync(Guid id, AssetItem updateModel)
    {
        try
        {
            await _assetItemRepository.UpdateAsync(id, updateModel);
        }
        catch (Exception e)
        {
            Error.Failure("AssetItem.Update");
        }
    }

    public async Task RemoveAsync(Guid id)
    {
        try
        {
            await _assetItemRepository.RemoveAsync(id);
        }
        catch (Exception e)
        {
            Error.Failure("AssetItem.Remove");
        }
    }
}
