namespace TuDa.CIMS.Shared.Models;

/// <summary>
/// Represents the statistics of consumables.
/// </summary>
public record ConsumableStatistics
{
    /// <summary>
    /// The amount of consumables from the previous year.
    /// </summary>
    public int PreviousYearAmount { get; set; }

    /// <summary>
    /// The total amount of consumables added.
    /// </summary>
    public int TotalAdded { get; set; }

    /// <summary>
    /// The total amount of consumables removed.
    /// </summary>
    public int TotalRemoved { get; set; }

    /// <summary>
    /// The current amount of consumables.
    /// </summary>
    public int CurrentAmount { get; set; }
}
