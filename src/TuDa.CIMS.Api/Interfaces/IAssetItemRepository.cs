using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemRepository
{
    public Task<AssetItem> Get(Guid id);

    /// <summary>
    /// TEst
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<AssetItem>> GetAll();

    /// <summary>
    /// Returns
    /// </summary>
    /// <param name="assetItems"></param>
    /// <returns></returns>
    public Task AddAll(IEnumerable<AssetItem> assetItems);
}
