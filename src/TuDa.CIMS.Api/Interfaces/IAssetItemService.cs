using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemService
{
    Task<ErrorOr<IEnumerable<AssetItem>>> GetAllAsync();
    Task<ErrorOr<AssetItem>> GetOneAsync(Guid id);
    Task<ErrorOr<Success>> UpdateAsync(Guid id, AssetItem updateModel);
    Task<ErrorOr<Success>> RemoveAsync(Guid id);
}
