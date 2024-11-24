using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemRepository
{
    Task<IEnumerable<AssetItem>> GetAll();
    Task<AssetItem> GetOne(Guid id);
    Task Update(Guid id);
    Task Remove(Guid id);
}
