namespace TuDa.CIMS.Shared.Entities;

public record AssetItem()
{
    public string Cas { get; set; }
    public Guid Id { get; set; }
    public string Note { get; set; }
    public Room? Room { get; set; }
    public List<Hazard>? Hazards { get; set; }
};
