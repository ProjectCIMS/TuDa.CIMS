using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemService
{
    Task<IEnumerable<AssetItem>> GetAll();
    Task<ErrorOr<AssetItem>> GetOne(Guid id);
    Task Update(Guid id);
    Task Remove(Guid id);
}
