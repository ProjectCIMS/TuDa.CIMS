using Microsoft.EntityFrameworkCore;
using TuDa.CIMS.Shared.Dtos;

namespace TuDa.CIMS.Api.Factories;

public static class PaginatedResponseFactory<T>
{
    /// <summary>
    /// method to create a paginated response.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static async Task<PaginatedResponse<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var countTask = source.CountAsync();
        var itemsTask = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        Task.WaitAll(countTask, itemsTask);

        return new PaginatedResponse<T>(await itemsTask, await countTask, pageNumber, pageSize);
    }
}
