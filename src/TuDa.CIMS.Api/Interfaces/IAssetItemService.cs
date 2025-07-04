﻿using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Dtos.Create;
using TuDa.CIMS.Shared.Dtos.Responses;
using TuDa.CIMS.Shared.Dtos.Update;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Params;

namespace TuDa.CIMS.Api.Interfaces;

public interface IAssetItemService
{
    Task<ErrorOr<List<AssetItem>>> GetAllAsync(AssetItemFilterDto filter);
    Task<ErrorOr<AssetItem>> GetOneAsync(Guid id);
    Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateAssetItemDto updateModel);
    Task<ErrorOr<Deleted>> RemoveAsync(Guid id);
    Task<ErrorOr<PaginatedResponse<AssetItem>>> GetPaginatedAsync(
        AssetItemPaginationQueryParams queryParams
    );

    Task<ErrorOr<Created>> CreateAsync(CreateAssetItemDto createModel);
}
