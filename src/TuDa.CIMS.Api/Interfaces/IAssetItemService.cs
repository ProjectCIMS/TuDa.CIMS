using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemService
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<AssetItem>> GetAll();
}
