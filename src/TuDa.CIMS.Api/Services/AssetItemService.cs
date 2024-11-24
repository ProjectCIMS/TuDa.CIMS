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

    public async Task<ErrorOr<AssetItem>> Get(Guid id)
    {
        try
        {
            return await _assetItemRepository.Get(id);
        }
        catch (Exception e)
        {
            return Error.Failure("AssetItem.Get");
        }
    }

    public async Task<ErrorOr<IEnumerable<AssetItem>>> GetAll()
    {
        try
        {
            var result = await _assetItemRepository.GetAll();
            return result.ToErrorOr();
        }
        catch (Exception e)
        {
            return Error.Failure();
        }
    }
}
