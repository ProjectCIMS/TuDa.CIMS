using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemRepository
{
    Task<IEnumerable<AssetItem>> GetAllAsync();
    Task<AssetItem> GetOneAsync(Guid id);
    Task UpdateAsync(Guid id, AssetItem updateModel);
    Task RemoveAsync(Guid id);
}
