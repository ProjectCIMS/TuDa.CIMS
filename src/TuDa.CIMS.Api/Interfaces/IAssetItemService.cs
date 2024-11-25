using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemService
{
    Task<IEnumerable<AssetItem>> GetAllAsync();
    Task<ErrorOr<AssetItem>> GetOneAsync(Guid id);
    Task UpdateAsync(Guid id, AssetItem updateModel);
    Task RemoveAsync(Guid id);
}
