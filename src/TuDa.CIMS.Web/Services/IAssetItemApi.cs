using Refit;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Web.Services;

[RefitClient("/api/asset-items")]
public interface IAssetItemApi
{
    [Get("/{id}")]
    public Task<AssetItem> GetAsync(Guid id);

    [Get("/")]
    public Task<IEnumerable<AssetItem>> GetAllAsync();

    [Put("/")]
    public Task<Guid> CreateAsync(AssetItem item);

    [Get("/{id}")]
    public Task RemoveAsync(Guid id);

    [Patch("/{id}")]
    public Task<AssetItem> UpdateAsync(Guid id, AssetItem item);
}
