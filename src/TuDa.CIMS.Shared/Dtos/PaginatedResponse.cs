using Microsoft.EntityFrameworkCore;

namespace TuDa.CIMS.Shared.Dtos;

/// <summary>
/// A class that represents a paginated response.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedResponse<T> : List<T>
{
    public PaginatedResponse(IEnumerable<T> currentPage, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(currentPage);
    }

    /// <summary>
    /// The current page of the paginated response.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// The total number of pages in the paginated response.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// The size of the page in the paginated response.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The total number of items in the paginated response.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// method to create a paginated response.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static async Task<PaginatedResponse<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedResponse<T>(items, count, pageNumber, pageSize);
    }
}
