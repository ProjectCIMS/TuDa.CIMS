using System.Runtime.InteropServices;

namespace TuDa.CIMS.Shared.Entities.Enums;

public enum Gender
{
    Unknown,
    Male,
    Female,
}

public static class GenderExtensions
{
    public static string ToSalution(this Gender gender) =>
        gender switch
        {
            Gender.Unknown => string.Empty,
            Gender.Male => "Herrn",
            Gender.Female => "Frau",
        };
}
