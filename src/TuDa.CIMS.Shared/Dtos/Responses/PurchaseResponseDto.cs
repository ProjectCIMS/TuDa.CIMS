using System.Text.Json.Serialization;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Dtos.Responses;

public record PurchaseResponseDto : BaseEntityResponseDto
{
    /// <summary>
    /// The person that purchase the items.
    /// </summary>
    public required Person Buyer { get; set; }

    /// <summary>
    /// All entries of the purchase.
    /// </summary>
    public List<PurchaseEntry> Entries { get; set; } = [];

    /// <summary>
    /// The date of completion.
    /// </summary>
    public DateTime? CompletionDate { get; set; }

    #region Invalidation

    /// <summary>
    /// Not null when Purchase is invalidated.
    /// The purchase this purchase is corrected with.
    /// </summary>
    public Guid? SuccessorId { get; set; }

    /// <summary>
    /// The purchase this purchase is correcting.
    /// </summary>
    public Guid? PredecessorId { get; set; }

    /// <summary>
    /// If the purchase is invalidated to correct it with another one.
    /// </summary>
    [JsonIgnore]
    public bool Invalidated => SuccessorId is not null;

    #endregion

    #region Methods

    /// <summary>
    /// Calculates the TotalPrice of this purchase.
    /// </summary>
    [JsonIgnore]
    public double TotalPrice => Entries.Aggregate(0.0, (total, entry) => total + entry.TotalPrice);

    #endregion
}
