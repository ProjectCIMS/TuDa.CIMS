namespace TuDa.CIMS.Shared.Entities.Enums;

public enum Gender
{
    Unknown,
    Male,
    Female,
    Divers,
}

public static class GenderExtensions
{
    public static string ToSalution(this Gender gender) =>
        gender switch
        {
            Gender.Unknown or Gender.Divers => string.Empty,
            Gender.Male => "Herrn",
            Gender.Female => "Frau",
        };
}
