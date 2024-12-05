namespace TuDa.CIMS.Shared.Params;

/// <summary>
/// A class that represents the parameters for pagination.
/// </summary>
public class UserParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;

    /// <summary>
    /// A method to get and set the page size.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}
