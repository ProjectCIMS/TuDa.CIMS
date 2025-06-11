using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Dtos.Update;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Params;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemRepository
{
    Task<List<AssetItem>> GetAllAsync();
    Task<AssetItem?> GetOneAsync(Guid id);
    Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateAssetItemDto updateModel);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id);
    Task<ErrorOr<PaginatedResponse<AssetItem>>> GetPaginatedAsync(
        AssetItemPaginationQueryParams queryParams
    );
    Task<List<AssetItem>> SearchAsync(string nameOrCas, List<AssetItemType>? assetItemTypes);
    Task<List<AssetItem>> FilterTypeAsync(List<AssetItemType> assetItemTypes);
    Task<List<AssetItem>> FilterAsync(AssetItemFilterDto filter);
    Task<List<AssetItem>> CombinedFilterAsync(AssetItemFilterDto filter);

    Task<ErrorOr<Created>> CreateAsync(CreateAssetItemDto createModel);
}
