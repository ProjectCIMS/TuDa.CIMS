using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemService
{
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Task<ErrorOr<IEnumerable<AssetItem>>> GetAll();
}
