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

    public async Task<IEnumerable<AssetItem>> GetAll()
    {
        try
        {
            var result = await _assetItemRepository.GetAll();
            return result;
        }
        catch (Exception e)
        {
            return [];
        }
    }

    public async Task<ErrorOr<AssetItem>> GetOne(Guid id)
    {
        try
        {
            return await _assetItemRepository.GetOne(id);
        }
        catch (Exception e)
        {
            return Error.Failure("AssetItem.GetOne");
        }
    }

    public async Task Update(Guid id)
    {
        try
        {
            await _assetItemRepository.Update(id);
        }
        catch (Exception e)
        {
            Error.Failure("AssetItem.Update");
        }
    }

    public async Task Remove(Guid id)
    {
        try
        {
            await _assetItemRepository.Remove(id);
        }
        catch (Exception e)
        {
            Error.Failure("AssetItem.Remove");
        }
    }
}
