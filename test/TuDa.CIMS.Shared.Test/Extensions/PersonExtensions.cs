using TuDa.CIMS.Shared.Dtos;
using TuDa.CIMS.Shared.Entities;

namespace TuDa.CIMS.Shared.Test.Extensions;

public static class PersonExtensions
{
    public static CreateProfessorDto ToCreateDto(this Professor professor) =>
        new()
        {
            Name = professor.Name,
            FirstName = professor.FirstName,
            Gender = professor.Gender,
            Title = professor.Title,
            AddressStreet = professor.Address.Street,
            AddressNumber = professor.Address.Number,
            AddressZipCode = professor.Address.ZipCode,
            AddressCity = professor.Address.City,
        };
}
