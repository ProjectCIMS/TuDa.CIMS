using Refit;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Params;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemService
{
    Task<ErrorOr<List<AssetItem>>> GetAllAsync(
        string? nameOrCas,
        List<AssetItemType>? assetItemTypes
    );
    Task<ErrorOr<AssetItem>> GetOneAsync(Guid id);
    Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateAssetItemDto updateModel);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id);
    Task<ErrorOr<PaginatedResponse<AssetItem>>> GetPaginatedAsync(
        AssetItemPaginationQueryParams queryParams
    );

    Task<ErrorOr<Created>> CreateAsync(CreateAssetItemDto createModel);
}
