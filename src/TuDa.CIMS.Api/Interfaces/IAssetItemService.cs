﻿using Refit;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Params;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemService
{
    Task<ErrorOr<IEnumerable<AssetItem>>> GetAllAsync(string? nameOrCas);
    Task<ErrorOr<AssetItem>> GetOneAsync(Guid id);
    Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateAssetItemDto updateModel);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id);
    Task<ErrorOr<PaginatedResponse<AssetItem>>> GetPaginatedAsync(AssetItemPaginationQueryParams queryParams);
}
