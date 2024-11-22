
using SixLabors.ImageSharp;

namespace TuDa.CIMS.Shared.Entities;

public record Hazard()
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Image Image { get; set; }
};
