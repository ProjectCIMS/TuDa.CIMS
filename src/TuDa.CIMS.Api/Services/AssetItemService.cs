using System.Runtime.InteropServices.JavaScript;
using Refit;
using TuDa.CIMS.Api.Interfaces;
using TuDa.CIMS.Shared.Attributes.ServiceRegistration;
using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;
using TuDa.CIMS.Shared.Entities.Enums;
using TuDa.CIMS.Shared.Params;

namespace TuDa.CIMS.Api.Services;

[ScopedService]
public class AssetItemService : IAssetItemService
{
    private readonly IAssetItemRepository _assetItemRepository;

    public AssetItemService(IAssetItemRepository assetItemRepository)
    {
        _assetItemRepository = assetItemRepository;
    }

    /// <summary>
    /// Return an an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetAllAsync"/> functionality if successful or if nameOrCas is set the <see cref="SearchAsync"/> functionality.
    /// </summary>
    public async Task<ErrorOr<List<AssetItem>>> GetAllAsync(
        string? nameOrCas,
        List<AssetItemType>? assetItemTypes,
        Dictionary<string, string>? filters
    )
    {
        try
        {
            if (!string.IsNullOrEmpty(nameOrCas))
            {
                return await _assetItemRepository.SearchAsync(nameOrCas, assetItemTypes);
            }

            if (assetItemTypes != null && assetItemTypes.Count > 0)
            {
                if (filters != null && filters.Count > 0)
                {
                    return await _assetItemRepository.CombinedFilterAsync(filters, assetItemTypes);
                }
                return await _assetItemRepository.FilterTypeAsync(assetItemTypes);
            }

            if (filters != null && filters.Count > 0)
            {
                return await _assetItemRepository.FilterAsync(filters);
            }

            return await _assetItemRepository.GetAllAsync();
        }
        catch (Exception e)
        {
            return nameOrCas != null
                ? Error.Failure(
                    "AssetItem.SearchAsync",
                    $"Failed to search AssetItem with name {nameOrCas}. Exception: {e.Message}"
                )
                : Error.Failure(
                    "AssetItem.GetAllAsync",
                    $"Failed to get all AssetItems. Exception: {e.Message}"
                );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetOneAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    public async Task<ErrorOr<AssetItem>> GetOneAsync(Guid id)
    {
        try
        {
            return await _assetItemRepository.GetOneAsync(id) switch
            {
                null => Error.NotFound(
                    "AssetItem.GetOneAsync",
                    $"AssetItem with id {id} not found."
                ),
                var value => value,
            };
        }
        catch (Exception e)
        {
            return Error.Unexpected(
                "AssetItem.GetOneAsync",
                $"An unexpected error occurred: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Return an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="UpdateAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    /// <param name="updateModel">the model containing the updated values for the AssetItem </param>
    public async Task<ErrorOr<Updated>> UpdateAsync(Guid id, UpdateAssetItemDto updateModel)
    {
        try
        {
            return await _assetItemRepository.UpdateAsync(id, updateModel);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "AssetItem.UpdateAsync",
                $"Failed to update AssetItem with ID {id}. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="RemoveAsync"/> functionality if successful
    /// </summary>
    /// <param name="id">the unique id of the AssetItem</param>
    public async Task<ErrorOr<Deleted>> RemoveAsync(Guid id)
    {
        try
        {
            return await _assetItemRepository.RemoveAsync(id);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "AssetItem.RemoveAsync",
                $"Failed to remove AssetItem with ID {id}. Exception: {e.Message}"
            );
        }
    }

    /// <summary>
    /// Returns an <see cref="ErrorOr{T}"/> that either contains an error message if an error occurs,
    /// or the result of the <see cref="GetPaginatedAsync"/> functionality if successful
    /// </summary>
    /// <param name="userParams"></param>
    /// <returns></returns>
    public async Task<ErrorOr<PaginatedResponse<AssetItem>>> GetPaginatedAsync(
        AssetItemPaginationQueryParams queryParams
    )
    {
        try
        {
            return await _assetItemRepository.GetPaginatedAsync(queryParams);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "AssetItem.GetPaginatedAsync",
                $"Failed to get paginated AssetItems. Exception: {e.Message}"
            );
        }
    }

    public async Task<ErrorOr<Created>> CreateAsync(CreateAssetItemDto createModel)
    {
        try
        {
            return await _assetItemRepository.CreateAsync(createModel);
        }
        catch (Exception e)
        {
            return Error.Failure(
                "AssetItem.CreateAsync",
                $"Failed to create AssetItem. Exception: {e.Message}"
            );
        }
    }
}
